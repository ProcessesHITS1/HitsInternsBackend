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
}


public class CompaniesService : ICompaniesService
{
    private readonly InterviewDbContext _context;
    private readonly CompaniesClient _companiesClient;
    private readonly ILogger<CompaniesService> _logger;

    public CompaniesService(InterviewDbContext context, CompaniesClient companiesClient, ILogger<CompaniesService> logger)
    {
        _context = context;
        _companiesClient = companiesClient;
        _logger = logger;
    }

    public async Task<CompanyInSeasonInfo> Create(Guid id, Season season)
    {
        // check that company exists
        var company = await _companiesClient.Get(id);

        // check that company exists in season
        if (season.Companies.Any(c => c.Id == id))
            throw new BadRequestException($"Company with id {id} already exists in season with year {season.Year}");

        season.Companies.Add(new Company
        {
            Id = company.Id,
            Name = company.Name,
            Season = season
        });

        await _context.SaveChangesAsync();
        return new CompanyInSeasonInfo
        {
            Id = company.Id,
            Name = company.Name,
            SeasonYear = season.Year,
            NPositions = 0
        };
    }


    public async Task Delete(Guid id, Season season)
    {
        // check that company exists (is it necessary?)
        var companyId = (await _companiesClient.Get(id)).Id;

        var res = season.Companies.Remove(season.Companies.First(c => c.Id == companyId));
        if (!res) _logger.LogWarning($"Company with id {companyId} was not found in season with id {season.Id}");
        await _context.SaveChangesAsync();
    }
}