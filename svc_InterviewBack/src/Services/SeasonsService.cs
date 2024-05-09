using AutoMapper;
using Microsoft.EntityFrameworkCore;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;


using Season = Models.Season;
using SeasonDb = DAL.Season;

// Here input and output models are both DTOs
public interface ISeasonsService
{
    public Task<List<Season>> GetAll();
    public Task<SeasonDetails> Get(int year);
    public Task<Season> Create(SeasonData seasonData);
    public Task<Season> Update(int year, SeasonData seasonData);
    public Task Delete(int year);
}

public class SeasonsService : ISeasonsService
{
    private readonly InterviewDbContext _context;
    private readonly IMapper _mapper;

    public SeasonsService(InterviewDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Season> Create(SeasonData seasonData)
    {
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
        var season = await _context.Seasons
                .Include(s => s.Students)
                .Include(s => s.Companies)
                .FirstOrDefaultAsync(s => s.Year == year)
                ?? throw new NotFoundException($"Season with year {year} not found");
        return _mapper.Map<SeasonDetails>(season);
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