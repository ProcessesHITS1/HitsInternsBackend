using AutoMapper;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;

namespace svc_InterviewBack.Services;


using Season = Models.Season;
using SeasonDb = DAL.Season;

// Here input and output models are both DTOs
public interface ISeasonsService
{
    public Task<List<Season>> GetAll();
    public Task<SeasonDetails> Get(int year);
    public Task<Season> Create(SeasonData seasonData);
    public Task<Season> Update(Guid id, SeasonData seasonData);
    public Task Delete(Guid id);
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
        await Task.WhenAll(_context.AddAsync(season).AsTask(), _context.SaveChangesAsync());

        return _mapper.Map<Season>(season);
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<SeasonDetails> Get(int year)
    {
        throw new NotImplementedException();
    }

    public Task<List<Season>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Season> Update(Guid id, SeasonData seasonData)
    {
        throw new NotImplementedException();
    }
}