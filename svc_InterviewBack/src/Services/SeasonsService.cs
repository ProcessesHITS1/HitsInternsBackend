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
    public Task<SeasonDetails> Get(int year);
    public Task<Season> Create(SeasonData seasonData);
    public Task<Season> Update(int year, SeasonData seasonData);
    public Task Delete(int year);

    // extra methods for internal use
    public Task<SeasonDb> Find(int year, bool withCompanies = true, bool withStudents = true);
}

public class SeasonsService(InterviewDbContext context, IMapper mapper) : ISeasonsService
{
    private readonly InterviewDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Season> Create(SeasonData seasonData)
    {
        if (seasonData.Year < 2010 || seasonData.Year > 3000)
            throw new BadRequestException("Invalid year value. Year should be between 2010 and 3000");
        if (seasonData.SeasonStart >= seasonData.SeasonEnd)
            throw new BadRequestException("Season start date should be before season end date");
        if (await _context.Seasons.AnyAsync(s => s.Year == seasonData.Year))
            throw new BadRequestException($"Season with year {seasonData.Year} already exists");
        var season = _mapper.Map<SeasonDb>(seasonData);
        _context.Add(season);
        await _context.SaveChangesAsync();
        return _mapper.Map<Season>(season);
    }

    public async Task Delete(int year)
    {
        var season = await _context.Seasons.FirstOrDefaultAsync(s => s.Year == year)
                    ?? throw new NotFoundException($"Season with year {year} not found");
        _context.Remove(season);
        await _context.SaveChangesAsync();
    }

    public async Task<SeasonDetails> Get(int year)
    {
        var season = await Find(year);
        return _mapper.Map<SeasonDetails>(season);//TODO: positions not included
    }

    public async Task<SeasonDb> Find(int year, bool withCompanies = true, bool withStudents = true)
    {
        var query = _context.Seasons.AsQueryable();

        if (withCompanies)
            query = query.Include(s => s.Companies).ThenInclude(c => c.Positions);
        if (withStudents)
            query = query.Include(s => s.Students);
        
        var season = await query.FirstOrDefaultAsync(s=>s.Year==year)
                ?? throw new NotFoundException($"Season with year {year} not found");
        return season;
    }

    public async Task<List<Season>> GetAll()
    {
        var seasons = await _context.Seasons.ToListAsync();
        return _mapper.Map<List<Season>>(seasons);
    }

    public async Task<Season> Update(int year, SeasonData seasonData)
    {
        var season = await _context.Seasons.FirstOrDefaultAsync(s => s.Year == year)
                ?? throw new NotFoundException($"Season with year {year} not found");
        _mapper.Map(seasonData, season);
        await _context.SaveChangesAsync();
        return _mapper.Map<Season>(season);
    }
}