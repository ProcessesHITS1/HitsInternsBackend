using AutoMapper;
using Interns.Common;
using Microsoft.EntityFrameworkCore;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;

namespace svc_InterviewBack.Services;

public interface IRequestStatusTemplateService
{
    Task CreateRequestStatusInSeason(int year, string statusName);
    Task<List<RequestStatusTemplateData>> GetRequestStatusesInSeason(int year);
}

public class RequestStatusTemplateService(InterviewDbContext context, IMapper mapper) : IRequestStatusTemplateService
{
    public async Task CreateRequestStatusInSeason(int year, string statusName)
    {
        // Check if the season exists for the given year
        var season = await context.Seasons.Include(s => s.RequestStatusTemplates)
            .FirstOrDefaultAsync(s => s.Year == year);
        if (season == null)
        {
            throw new NotFoundException($"Season for year {year} not found");
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
        season.RequestStatusTemplates ??= [];
        if (season.RequestStatusTemplates.All(rs => rs.Name != statusName))
        {
            season.RequestStatusTemplates.Add(statusTemplate);
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<RequestStatusTemplateData>> GetRequestStatusesInSeason(int year)
    {
        var season = await context.Seasons.Include(s => s.RequestStatusTemplates)
            .FirstOrDefaultAsync(s => s.Year == year);
        if (season == null)
        {
            throw new NotFoundException($"Season for year {year} not found");
        }

        return season.RequestStatusTemplates
            .Select(mapper.Map<RequestStatusTemplateData>)
            .ToList();
    }
}