using AutoMapper;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services.Clients;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services;


using Season = DAL.Season;
using Role = UsersClient.Role;

public interface IStudentsService
{
    public Task<StudentInfo> Create(Guid id, Season season);
    public Task Delete(Guid id, Season season);
}


public class StudentsService(InterviewDbContext context, UsersClient usersClient, ILogger<CompaniesService> logger, IMapper mapper) : IStudentsService
{
    private readonly InterviewDbContext _context = context;
    private readonly UsersClient _usersClient = usersClient;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CompaniesService> _logger = logger;

    public async Task<StudentInfo> Create(Guid id, Season season)
    {
        // check that student exists
        var user = await _usersClient.GetUser(id);
        if (!user.Roles.Contains(Role.ROLE_STUDENT))
            throw new BadRequestException($"User with id {id} is not a student");

        // check that student exists in season
        if (season.Students.Any(c => c.Id == id))
            throw new BadRequestException($"Student with id {id} already exists in season with year {season.Year}");

        var student = new Student
        {
            Id = user.Id,
            Name = user.LastName + " " + user.FirstName + " " + user.Patronymic,
            Season = season,
            EmploymentStatus = EmploymentStatus.Unemployed,
            InterviewRequests = []
        };
        _context.Students.Add(student);

        await _context.SaveChangesAsync();
        return _mapper.Map<StudentInfo>(student);
    }


    public async Task Delete(Guid id, Season season)
    {
        var res = season.Students.Remove(season.Students.First(c => c.Id == id));
        if (!res) _logger.LogWarning($"Student with id {id} was not found in season with year {season.Year}");
        await _context.SaveChangesAsync();
    }
}