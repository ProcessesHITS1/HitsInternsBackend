using AutoMapper;
using Interns.Common;
using Interns.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;

namespace svc_InterviewBack.Services;

public interface IRequestService
{
    Task<RequestDetails> Create(Guid studentId, Guid positionId, Guid reqStatusId);

    Task<RequestDetails> UpdateResultStatus(Guid requestId, Guid userId, bool isStudent, RequestResultUpdate reqResult);

    Task<RequestDetails> UpdateRequestStatus(Guid requestId, Guid newRequestStatusId);

    Task<PaginatedItems<RequestData>> GetRequests(RequestQuery requestQuery,
        int page, int pageSize);

    Task<RequestData> GetRequest(Guid requestId, Guid? userId, bool isStudent);
}

public class RequestService(InterviewDbContext context, IMapper mapper) : IRequestService
{
    public async Task<RequestDetails> Create(Guid studentId, Guid positionId, Guid reqStatusId)
    {
        var student = await context.Students
            .Include(s => s.InterviewRequests).ThenInclude(interviewRequest => interviewRequest.Position)
            .Include(s => s.Season)
            .ThenInclude(se => se.RequestStatusTemplates)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            throw new NotFoundException($"Student was not found, id:{studentId}");
        }

        var position = await context.Positions
            .FirstOrDefaultAsync(p => p.Id == positionId);
        if (position == null) throw new NotFoundException($"Position was not found, id:{positionId}");

        if (student.InterviewRequests.Any(x => x.Position.Id == position.Id))
        {
            throw new InvalidOperationException($"User already applied to this position");
        }

        var interviewRequest = new InterviewRequest
        {
            Student = student,
            Position = position,
        };


        var season = student.Season;

        // Find the initial status in season
        var initialStatusTemplate = season.RequestStatusTemplates?.Find(rst => rst.Id == reqStatusId);

        if (initialStatusTemplate == null)
        {
            throw new NotFoundException($"Initial request status 'Waiting' not found in season {season.Year}");
        }

        var newSnapshot = new RequestStatusSnapshot
        {
            DateTime = DateTime.UtcNow,
            RequestStatusTemplate = initialStatusTemplate,
            InterviewRequest = interviewRequest
        };

        interviewRequest.RequestStatusSnapshots.Add(newSnapshot);

        context.InterviewRequests.Add(interviewRequest);
        context.RequestStatusSnapshots.Add(newSnapshot);

        await context.SaveChangesAsync();

        var interviewRequestDto = mapper.Map<RequestDetails>(interviewRequest);

        return interviewRequestDto;
    }


    public async Task<RequestDetails> UpdateRequestStatus(Guid requestId, Guid newRequestStatusId)
    {
        var request = await context.InterviewRequests
                          .Include(r => r.Student)
                          .ThenInclude(s => s.Season)
                          .ThenInclude(se => se.RequestStatusTemplates)
                          .FirstOrDefaultAsync(r => r.Id == requestId) ??
                      throw new NotFoundException($"Request {requestId} not found");
        var season = request.Student.Season;

        // Check if the new request status exists in the season
        var statusTemplate = season.RequestStatusTemplates?.Find(st => st.Id == newRequestStatusId);
        //TODO: fix response
        if (statusTemplate == null)
        {
            throw new NotFoundException(
                $"Request status with Id '{newRequestStatusId}' not found in season {season.Year}");
        }

        var newSnapshot = new RequestStatusSnapshot
        {
            DateTime = DateTime.UtcNow,
            RequestStatusTemplate = statusTemplate,
            InterviewRequest = request
        };

        request.RequestStatusSnapshots.Add(newSnapshot);

        await context.SaveChangesAsync();

        return mapper.Map<RequestDetails>(request);
    }


    public async Task<RequestDetails> UpdateResultStatus(Guid requestId, Guid userId, bool isStudent,
        RequestResultUpdate reqResult)
    {
        var request = await context.InterviewRequests
                          .Include(r => r.Position)
                          .ThenInclude(p => p.Company)
                          .Include(r => r.RequestResult)
                          .Include(interviewRequest => interviewRequest.Student)
                          .FirstOrDefaultAsync(r => r.Id == requestId) ??
                      throw new NotFoundException($"Request {requestId} not found");

        return await UpdateResultStatus(request, reqResult, userId, isStudent);
    }

    private async Task<RequestDetails> UpdateResultStatus(InterviewRequest request, RequestResultUpdate resultUpdateDto,
        Guid userId, bool isStudent)
    {
        if (isStudent) resultUpdateDto.SchoolResultStatus = null;
        else resultUpdateDto.StudentResultStatus = null;

        var studentId = request.Student.Id;

        if (isStudent && studentId != userId)
        {
            throw new AccessDeniedException(
                $"Access denied: User {userId} is not authorized to access request with id: {request.Id}.");
        }

        if (!isStudent && studentId == userId)
        {
            throw new AccessDeniedException(
                $"Access denied: Staff {userId} is not authorized to change status of its own request with id: {request.Id}.");
        }

        var seasonId = request.Student.SeasonId;

        var hasAlreadyConfirmed = await context.InterviewRequests.AnyAsync(x =>
            x.Id != request.Id &&
            x.Student.Id == studentId &&
            x.Student.SeasonId == seasonId &&
            x.RequestResult != null &&
            ((isStudent && x.RequestResult.StudentResultStatus == ResultStatus.Accepted) ||
             (!isStudent && x.RequestResult.SchoolResultStatus == ResultStatus.Accepted)));

        if (hasAlreadyConfirmed)
        {
            throw new BadRequestException($"{(isStudent ? "Student" : "Staff")} already confirmed another request");
        }

        await CreateOrUpdateRequestResult(request, resultUpdateDto);
        return mapper.Map<RequestDetails>(request);
    }

    private async Task CreateOrUpdateRequestResult(InterviewRequest request, RequestResultUpdate dto)
    {
        request.RequestResult ??= new();

        var student = request.Student;

        var isRequestEmploymentFlagsUpdated = request.RequestResult.StudentResultStatus != dto.StudentResultStatus ||
                                              request.RequestResult.SchoolResultStatus != dto.SchoolResultStatus ||
                                              request.RequestResult.OfferGiven != dto.OfferGiven;
        if (request.RequestResult.IsEmployed() && isRequestEmploymentFlagsUpdated)
        {
            student.EmploymentStatus = EmploymentStatus.Unemployed;
            student.Company = null;
        }

        request.RequestResult.OfferGiven = dto.OfferGiven ?? request.RequestResult.OfferGiven;
        request.RequestResult.SchoolResultStatus = dto.SchoolResultStatus ?? request.RequestResult.SchoolResultStatus;
        request.RequestResult.StudentResultStatus = dto.StudentResultStatus ?? request.RequestResult.StudentResultStatus;
        request.RequestResult.Description = dto.Description;

        // update student employment status
        if (request.RequestResult.IsEmployed())
        {
            student.Company = request.Position.Company;
            student.EmploymentStatus = EmploymentStatus.Employed;
        }

        await context.SaveChangesAsync();
    }


    public Task<PaginatedItems<RequestData>> GetRequests(RequestQuery requestQuery, int page, int pageSize)
    {
        return context.Seasons
            .Where(x => requestQuery.SeasonYears.IsNullOrEmpty() || requestQuery.SeasonYears.Contains(x.Year))
            .SelectMany(x => x.Students)
            .Where(x => requestQuery.StudentIds.IsNullOrEmpty() || requestQuery.StudentIds.Contains(x.Id))
            .SelectMany(x => x.InterviewRequests)
            .Where(x => requestQuery.CompanyIds.IsNullOrEmpty() ||
                        requestQuery.CompanyIds.Contains(x.Position.Company.Id))
            .OrderByDescending(x => x.RequestStatusSnapshots.Max(s => s.DateTime))
            .Select(x => new
            {
                x.Id,
                x.Student,
                x.Position,
                CompanyId = x.Position.Company.Id,
                x.RequestStatusSnapshots,
                RequestStatusTemplates = x.RequestStatusSnapshots.Select(x => x.RequestStatusTemplate),
                x.RequestResult
            })
            .Paginated(
                page,
                pageSize,
                r =>
                {
                    var latestSnapshot = r.RequestStatusSnapshots.MaxBy(s => s.DateTime);
                    return new RequestData
                    {
                        Id = r.Id,
                        StudentId = r.Student.Id,
                        StudentName = r.Student.Name,
                        PositionId = r.Position.Id,
                        CompanyId = r.CompanyId,
                        PositionTitle = r.Position.Title,
                        RequestStatusSnapshots = requestQuery.IncludeHistory
                            ? r.RequestStatusSnapshots.OrderByDescending(s => s.DateTime).Select(s =>
                                new RequestStatusSnapshotData
                                {
                                    Id = s.Id,
                                    DateTime = s.DateTime,
                                    Status = s.RequestStatusTemplate.Name
                                }).ToList()
                            :
                            [
                                new RequestStatusSnapshotData
                                {
                                    Id = latestSnapshot?.Id ?? Guid.Empty,
                                    DateTime = latestSnapshot?.DateTime ?? DateTime.MinValue,
                                    Status = latestSnapshot?.RequestStatusTemplate.Name ?? string.Empty
                                }
                            ],
                        RequestResult = mapper.Map<RequestResultData>(r.RequestResult)
                    };
                }
            );
    }

    public async Task<RequestData> GetRequest(Guid requestId, Guid? userId, bool isStudent)
    {
        var request = await context.InterviewRequests.Include(ir => ir.Student)
            .Include(ir => ir.Position)
            .Include(ir => ir.RequestStatusSnapshots)
            .ThenInclude(s => s.RequestStatusTemplate)
            .Include(ir => ir.RequestResult).FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null)
            throw new NotFoundException($"Request with id: {requestId} , not found.");


        if (request.Student.Id != userId && isStudent)
            throw new AccessDeniedException(
                $"Access denied: User {userId} is not authorized to access request with id: {requestId}. ");

        return new RequestData
        {
            Id = request.Id,
            StudentId = request.Student.Id,
            StudentName = request.Student.Name,
            PositionId = request.Position.Id,
            PositionTitle = request.Position.Title,
            RequestStatusSnapshots = request.RequestStatusSnapshots.OrderByDescending(s => s.DateTime).Select(s =>
                new RequestStatusSnapshotData
                {
                    Id = s.Id,
                    DateTime = s.DateTime,
                    Status = s.RequestStatusTemplate.Name
                }).ToList(),

            RequestResult = mapper.Map<RequestResultData>(request.RequestResult)
        };
    }
}