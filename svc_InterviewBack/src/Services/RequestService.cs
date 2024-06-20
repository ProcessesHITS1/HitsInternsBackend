using AutoMapper;
using Interns.Common.Pagination;
using Microsoft.EntityFrameworkCore;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils;

using svc_InterviewBack.Utils.Extensions;

namespace svc_InterviewBack.Services;

public interface IRequestService
{
    Task<RequestDetails> Create(Guid studentId, Guid positionId, string statusName);
    Task<RequestDetails> UpdateResultStatus(Guid requestId, RequestResultUpdate reqResult);
    Task<RequestDetails> UpdateRequestStatus(Guid requestId, string newRequestStatus);
   

    Task<PaginatedItems<RequestData>> GetRequests(RequestQuery requestQuery,
        int page, int pageSize);
}

public class RequestService(InterviewDbContext context, IMapper mapper) : IRequestService
{
    public async Task<RequestDetails> Create(Guid studentId, Guid positionId, string statusName)
    {
        var student = await context.Students
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

        var reqResult = new RequestResult();
        
        var interviewRequest = new InterviewRequest
        {
            Student = student,
            Position = position,
            RequestResult = reqResult
        };

        
        var season = student.Season;

        // Find the initial status in season
        var initialStatusTemplate = season.RequestStatusTemplates?.FirstOrDefault(rst => rst.Name == statusName);

        if (initialStatusTemplate == null)
        {
            throw new KeyNotFoundException($"Initial request status 'Waiting' not found in season {season.Year}");
        }

        var newSnapshot = new RequestStatusSnapshot
        {
            DateTime = DateTime.UtcNow,
            RequestStatusTemplate = initialStatusTemplate,
            InterviewRequest = interviewRequest
        };

        interviewRequest.RequestStatusSnapshots.Add(newSnapshot);

        context.InterviewRequests.Add(interviewRequest);
        context.RequestResult.Add(reqResult);
        context.RequestStatusSnapshots.Add(newSnapshot);

        await context.SaveChangesAsync();

        var interviewRequestDto = mapper.Map<RequestDetails>(interviewRequest);

        return interviewRequestDto;
    }


    public async Task<RequestDetails> UpdateRequestStatus(Guid requestId, string newRequestStatus)
    {
        var request = await context.InterviewRequests
            .Include(r => r.RequestStatusSnapshots)
            .Include(r => r.Student)
            .ThenInclude(s => s.Season)
            .ThenInclude(se => se.RequestStatusTemplates)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) throw new NotFoundException($"Request {requestId} not found");

        var season = request.Student.Season;

        // Check if the new request status exists in the season
        var statusTemplate = season.RequestStatusTemplates?.First(rst => rst.Name == newRequestStatus);
        //TODO: fix response
        if (statusTemplate == null)
        {
            throw new KeyNotFoundException($"Request status '{newRequestStatus}' not found in season {season.Year}");
        }

        var newSnapshot = new RequestStatusSnapshot
        {
            DateTime = DateTime.UtcNow,
            RequestStatusTemplate = statusTemplate,
            InterviewRequest = request
        };

        request.RequestStatusSnapshots.Add(newSnapshot);

        context.RequestStatusSnapshots.Add(newSnapshot);
        await context.SaveChangesAsync();

        return mapper.Map<RequestDetails>(request);
    }


    public async Task<RequestDetails> UpdateResultStatus(Guid requestId, RequestResultUpdate reqResult)
    {
        var request = await context.InterviewRequests
            .Include(r => r.RequestResult)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) throw new KeyNotFoundException($"Request {requestId} not found");

        if (request.RequestResult == null)
        {
            request.RequestResult = mapper.Map<RequestResult>(reqResult);
            context.Add(request.RequestResult);
        }
        else
        {
            request.RequestResult.UpdateProperties(mapper.Map<RequestResult>(reqResult));
        }

        await context.SaveChangesAsync();
        return mapper.Map<RequestDetails>(request);
    }




    public async Task<PaginatedItems<RequestData>> GetRequests(RequestQuery requestQuery, int page, int pageSize)
    {
        // Base query for all interview requests
        var query = context.InterviewRequests
            .Include(ir => ir.Student)
            .Include(ir => ir.Position)
            .Include(ir => ir.RequestStatusSnapshots)
            .ThenInclude(s => s.RequestStatusTemplate)
            .Include(ir => ir.RequestResult)
            .AsQueryable();

        if (requestQuery.RequestIds != null && requestQuery.RequestIds.Count != 0)
        {
            query = query.Where(r => requestQuery.RequestIds.Contains(r.Id));
        }

        if (requestQuery.StudentIds != null && requestQuery.StudentIds.Count != 0)
        {
            query = query.Where(r => requestQuery.StudentIds.Contains(r.Student.Id));
        }

        // TODO: company filtering

        if (requestQuery.IncludeHistory)
        {
            query = query.Include(ir => ir.RequestStatusSnapshots)
                .OrderByDescending(ir => ir.RequestStatusSnapshots.Max(s => s.DateTime));
        }
        else
        {
            query = query.OrderByDescending(ir =>
                ir.RequestStatusSnapshots.OrderByDescending(s => s.DateTime).FirstOrDefault().DateTime);
        }

        var pagedRequests = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = pagedRequests.Select(r =>
        {
            var latestSnapshot = r.RequestStatusSnapshots.MaxBy(s => s.DateTime);
            return new RequestData
            {
                Id = r.Id,
                StudentId = r.Student.Id,
                StudentName = r.Student.Name,
                PositionId = r.Position.Id,
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
        }).ToList();

        var totalItems = await query.CountAsync();
        return new PaginatedItems<RequestData>
        {
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = page,
                TotalItems = totalItems,
                PageSize = pageSize
            },
            Items = result
        };
    }
}