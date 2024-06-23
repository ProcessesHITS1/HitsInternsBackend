using AutoMapper;
using Interns.Common;
using Interns.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils.Extensions;

namespace svc_InterviewBack.Services;

public interface IRequestService
{
    Task<RequestDetails> Create(Guid studentId, Guid positionId, Guid reqStatusId);

    Task<RequestDetails> UpdateResultStatus(Guid requestId, Guid userId, bool isStudent, bool isStaff,
        RequestResultUpdate reqResult);

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
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) throw new NotFoundException($"Request {requestId} not found");

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


    public async Task<RequestDetails> UpdateResultStatus(Guid requestId, Guid userId, bool isStudent, bool isStaff,
        RequestResultUpdate reqResult)
    {
        // Nullify status based on user role
        if (!isStaff) reqResult.SchoolResultStatus = null;
        if (!isStudent) reqResult.StudentResultStatus = null;

        var request = await context.InterviewRequests
            .Include(r => r.RequestResult).Include(interviewRequest => interviewRequest.Student)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        // Check if the request exists
        if (request == null) throw new NotFoundException($"Request {requestId} not found");

        // Authorization check
        ValidateUserAuthorization(isStaff, request, userId, requestId);

        var studentId = request.Student.Id;

        // Fetch requests for the student, except edited
        var studentRequests = await context.InterviewRequests.Where(r => r.Student.Id == studentId && r.Id!=requestId)
            .Include(interviewRequest => interviewRequest.RequestResult).ToListAsync();

        // Check for existing accepted requests
        CheckForExistingAcceptedRequests(studentRequests, reqResult, studentId);

        UpdateRequestResult(request, reqResult);

        await context.SaveChangesAsync();
        return mapper.Map<RequestDetails>(request);
    }

    private void ValidateUserAuthorization(bool isStaff, InterviewRequest request, Guid userId, Guid requestId)
    {
        if (!isStaff && request.Student.Id != userId)
            throw new AccessDeniedException(
                $"Access denied: User {userId} is not authorized to access request with id: {requestId}. ");
        if (isStaff && request.Student.Id == userId)
            throw new AccessDeniedException(
                $"Access denied: Staff {userId} is not authorized to change status of it's own request with id: {requestId}. ");
    }

    private void CheckForExistingAcceptedRequests(List<InterviewRequest> studentRequests, RequestResultUpdate reqResult,
        Guid studentId)
    {
        if (reqResult.StudentResultStatus != null)
        {
            var hasAccepted =
                studentRequests.Any(r => r.RequestResult is { StudentResultStatus: ResultStatus.Accepted });
            if (hasAccepted) throw new BadRequestException($"Student already confirmed other request");
        }

        if (reqResult.SchoolResultStatus != null)
        {
            var hasAccepted =
                studentRequests.Any(r => r.RequestResult is { StudentResultStatus: ResultStatus.Accepted });
            if (hasAccepted)
                throw new BadRequestException($"Staff already confirmed other request of the student {studentId}");
        }
    }

    private void UpdateRequestResult(InterviewRequest request, RequestResultUpdate reqResult)
    {
        if (request.RequestResult == null)
        {
            request.RequestResult = mapper.Map<RequestResult>(reqResult);
            context.Add(request.RequestResult);
        }
        else
        {
            request.RequestResult.UpdateProperties(mapper.Map<RequestResult>(reqResult));
        }
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