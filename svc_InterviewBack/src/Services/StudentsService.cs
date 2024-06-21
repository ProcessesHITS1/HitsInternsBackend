using AutoMapper;
using Interns.Common;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services.Clients;

namespace svc_InterviewBack.Services;


using Season = DAL.Season;
using Role = UsersClient.Role;

public interface IStudentsService
{
    public Task<StudentInfo> Create(Guid id, Season season);
    public Task Delete(Guid id, Season season);
    public List<StudentInfo> ConvertToStudentsInfo(List<Student> students);
}


public class StudentsService(InterviewDbContext context, UsersClient usersClient, ILogger<CompaniesService> logger, IMapper mapper) : IStudentsService
{
    public async Task<StudentInfo> Create(Guid id, Season season)
    {
        // check that student exists
        var user = await usersClient.GetUser(id);
        if (!user.Roles.Contains(Role.ROLE_STUDENT))
            throw new BadRequestException($"User with id {id} is not a student");

        // check that student exists in season
        if (season.Students.Any(c => c.Id == id))
            throw new BadRequestException($"Student with id {id} already exists in season with year {season.Year}");

        var student = new Student
        {
            Id = user.Id,
            Name = user.LastName + " " + user.FirstName + " " + user.Patronymic,
            SeasonId = season.Id,
            Season = season,
            EmploymentStatus = EmploymentStatus.Unemployed,
            InterviewRequests = []
        };
        context.Students.Add(student);

        await context.SaveChangesAsync();
        return mapper.Map<StudentInfo>(student);
    }


    public async Task Delete(Guid id, Season season)
    {
        var res = season.Students.Remove(season.Students.First(c => c.Id == id));
        if (!res) logger.LogWarning("Student with id {id} was not found in season with year {year}", id, season.Year);
        await context.SaveChangesAsync();
    }

    // TODO change to actual db query
    public List<StudentInfo> ConvertToStudentsInfo(List<Student> students)
    {
        return students.Select(mapper.Map<StudentInfo>).ToList();
    }
}