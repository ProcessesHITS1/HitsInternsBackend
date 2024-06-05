using AutoMapper;
using Microsoft.EntityFrameworkCore;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;

using Season = Models.Season;
using SeasonDb = DAL.Season;

public interface ISeasonsService
{
    // methods for clients
    // Here input and output models are both DTOs
    public Task<List<Season>> GetAll();
    public Task<Season> Create(SeasonData seasonData);
    public Task<Season> Update(int year, SeasonData seasonData);
    public Task Delete(int year);

    // extra methods for internal use
    public Task<SeasonDb> Find(int year, bool withCompanies = true, bool withStudents = true);
}

public class SeasonsService(InterviewDbContext context, IMapper mapper) : ISeasonsService
{
    public async Task<Season> Create(SeasonData seasonData)
    {
        if (seasonData.SeasonStart >= seasonData.SeasonEnd)
            throw new BadRequestException("Season start date should be before season end date");
        if (await context.Seasons.AnyAsync(s => s.Year == seasonData.Year))
            throw new BadRequestException($"Season with year {seasonData.Year} already exists");
        var season = mapper.Map<SeasonDb>(seasonData);
        context.Add(season);
        await context.SaveChangesAsync();
        return mapper.Map<Season>(season);
    }

    public async Task Delete(int year)
    {
        var season = await context.Seasons.FirstOrDefaultAsync(s => s.Year == year)
                     ?? throw new NotFoundException($"Season with year {year} not found");
        context.Remove(season);
        await context.SaveChangesAsync();
    }

    public async Task<SeasonDb> Find(int year, bool withCompanies = true, bool withStudents = true)
    {
        var query = context.Seasons.AsQueryable();
        if (withCompanies)
            query = query.Include(s => s.Companies);
        if (withStudents)
            query = query.Include(s => s.Students);

        var season = await query.FirstOrDefaultAsync(s => s.Year == year)
                     ?? throw new NotFoundException($"Season with year {year} not found");
        return season;
    }

    public async Task<List<Season>> GetAll()
    {
        var seasons = await context.Seasons.ToListAsync();
        return mapper.Map<List<Season>>(seasons);
    }

    public async Task<Season> Update(int year, SeasonData seasonData)
    {
        var season = await context.Seasons.FirstOrDefaultAsync(s => s.Year == year)
                     ?? throw new NotFoundException($"Season with year {year} not found");
        mapper.Map(seasonData, season);
        await context.SaveChangesAsync();
        return mapper.Map<Season>(season);
    }
}