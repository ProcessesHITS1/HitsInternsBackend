using AutoMapper;
using Microsoft.EntityFrameworkCore;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services.Clients;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;


using Season = DAL.Season;

public interface ICompaniesService
{
    public Task<CompanyInSeasonInfo> Create(Guid id, Season season);
    public Task Delete(Guid id, Season season);
    public Task<List<CompanyInSeasonInfo>> GetAll(int seasonYear);
}


public class CompaniesService(InterviewDbContext context, CompaniesClient companiesClient, ILogger<CompaniesService> logger, IMapper mapper) : ICompaniesService
{
    public async Task<CompanyInSeasonInfo> Create(Guid id, Season season)
    {
        // check that company exists
        var company = await companiesClient.Get(id);

        // check if company exists in season
        if (season.Companies.Any(c => c.Id == id))
            throw new BadRequestException($"Company with id {id} already exists in season with year {season.Year}");

        var companyInSeason = new Company
        {
            Id = company.Id,
            Name = company.Name,
            Season = season,
            Positions = []
        };
        context.Companies.Add(companyInSeason);

        await context.SaveChangesAsync();
        return mapper.Map<CompanyInSeasonInfo>(companyInSeason);
    }


    public async Task Delete(Guid id, Season season)
    {
        // check that company exists (is it necessary?)
        var companyId = (await companiesClient.Get(id)).Id;

        var res = season.Companies.Remove(season.Companies.First(c => c.Id == companyId));
        if (!res) logger.LogWarning($"Company with id {companyId} was not found in season with year {season.Year}");
        await context.SaveChangesAsync();
    }

    public async Task<List<CompanyInSeasonInfo>> GetAll(int seasonYear)
    {
        var companies = await context.Companies
            .Where(s => s.Season.Year == seasonYear)
            .Select(c => new CompanyInSeasonInfo
            {
                Id = c.Id,
                Name = c.Name,
                NPositions = c.Positions.Count,
                SeasonYear = c.Season.Year
            })
            .ToListAsync()
            ?? throw new NotFoundException($"Season with year {seasonYear} not found");

        return companies;
    }
}