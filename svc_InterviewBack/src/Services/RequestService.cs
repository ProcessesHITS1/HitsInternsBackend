using AutoMapper;
using Microsoft.EntityFrameworkCore;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;

public interface IRequestService
{
    Task<RequestDetails> CreateAsync(Guid studentId, Guid positionId, string statusName);
    Task<RequestDetails> UpdateResultStatus(Guid requestId, RequestResultData reqResult);
    Task<RequestDetails> UpdateRequestStatus(Guid requestId, string newRequestStatus);
    Task CreateRequestStatusInSeason(int year, string statusName);
    Task<List<RequestStatusTemplate>> GetRequestStatusesInSeason(int year);
    Task<object?> GetRequests();
}

public class RequestService(InterviewDbContext context, IMapper mapper) : IRequestService
{
    public async Task<RequestDetails> CreateAsync(Guid studentId, Guid positionId,string statusName)
    {
        var student = await context.Students
            .Include(s => s.Season)
            .ThenInclude(se => se.RequestStatuses)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            throw new NotFoundException($"Student was not found, id:{studentId}");
        }

        var position = await context.Positions
            .FirstOrDefaultAsync(p => p.Id == positionId);
        if (position == null) throw new NotFoundException($"Position was not found, id:{positionId}");

        var interviewRequest = new InterviewRequest
        {
            Id = Guid.NewGuid(),
            Student = student,
            Position = position,
        };

        var season = student.Season;

        // Find the initial status in season
        var initialStatusTemplate = season.RequestStatuses?.FirstOrDefault(rst => rst.Name == statusName);

        if (initialStatusTemplate == null)
        {
            throw new KeyNotFoundException($"Initial request status 'Waiting' not found in season {season.Year}");
        }

        var newSnapshot = new RequestStatusSnapshot
        {
            Id = Guid.NewGuid(),
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



    public async Task<RequestDetails> UpdateRequestStatus(Guid requestId, string newRequestStatus)
    {
        var request = await context.InterviewRequests
            .Include(r => r.RequestStatusSnapshots)
            .Include(r => r.Student)
            .ThenInclude(s => s.Season)
            .ThenInclude(se => se.RequestStatuses)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) throw new KeyNotFoundException($"Request {requestId} not found");

        var season = request.Student.Season;

        // Check if the new request status exists in the season
        var statusTemplate = season.RequestStatuses?.First(rst => rst.Name == newRequestStatus);
        //TODO: fix response
        if (statusTemplate == null)
        {
            throw new KeyNotFoundException($"Request status '{newRequestStatus}' not found in season {season.Year}");
        }

        var newSnapshot = new RequestStatusSnapshot
        {
            Id = Guid.NewGuid(),
            DateTime = DateTime.UtcNow,
            RequestStatusTemplate = statusTemplate,
            InterviewRequest = request
        };

        request.RequestStatusSnapshots.Add(newSnapshot);

        context.RequestStatusSnapshots.Add(newSnapshot);
        await context.SaveChangesAsync();

        return mapper.Map<RequestDetails>(request);
    }


    public async Task<RequestDetails> UpdateResultStatus(Guid requestId, RequestResultData reqResult)
    {
        var request = await context.InterviewRequests
            .Include(r => r.RequestResult)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null) throw new KeyNotFoundException($"Request {requestId} not found");

        if (request.RequestResult == null)
        {
            request.RequestResult = new RequestResult
            {
                Description = reqResult.Description,
                OfferGiven = reqResult.OfferGiven ?? false,
                ResultStatus = reqResult.ResultStatus ?? ResultStatus.Pending
            };
            context.Add(request.RequestResult);
        }
        else
        {
            if (reqResult.Description != null)
            {
                request.RequestResult.Description = reqResult.Description;
            }

            if (reqResult.OfferGiven.HasValue)
            {
                request.RequestResult.OfferGiven = reqResult.OfferGiven.Value;
            }

            if (reqResult.ResultStatus.HasValue)
            {
                request.RequestResult.ResultStatus = reqResult.ResultStatus.Value;
            }
        }

        await context.SaveChangesAsync();
        return mapper.Map<RequestDetails>(request);
    }


    public async Task CreateRequestStatusInSeason(int year, string statusName)
    {
        // Check if the season exists for the given year
        var season = await context.Seasons.Include(s => s.RequestStatuses)
            .FirstOrDefaultAsync(s => s.Year == year);
        if (season == null)
        {
            throw new KeyNotFoundException($"Season for year {year} not found");
        }

        // Check if the status already exists in the database
        var statusTemplate = await context.RequestStatusTemplates
            .FirstOrDefaultAsync(rst => rst.Name == statusName);
        if (statusTemplate == null)
        {
            // If the status doesn't exist, create it
            statusTemplate = new RequestStatusTemplate
            {
                Name = statusName
            };
            context.RequestStatusTemplates.Add(statusTemplate);
        }

        // Check if the status is already associated with the season
        season.RequestStatuses ??= [];
        if (season.RequestStatuses.All(rs => rs.Name != statusName))
        {
            season.RequestStatuses.Add(statusTemplate);
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<RequestStatusTemplate>> GetRequestStatusesInSeason(int year)
    {
        var season = await context.Seasons.Include(s => s.RequestStatuses)
            .FirstOrDefaultAsync(s => s.Year == year);
        if (season == null)
        {
            throw new KeyNotFoundException($"Season for year {year} not found");
        }

        return season.RequestStatuses?.ToList() ?? [];
    }

    public Task<object?> GetRequests()
    {
        //TODO:Add pagination
        throw new NotImplementedException();
    }
}