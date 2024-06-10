using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[Route("/api/season")]
[ApiController]
public class StudentsController(IStudentsService studentsService, ISeasonsService seasonsService) : ControllerBase
{
    private readonly IStudentsService _studentsService = studentsService;
    private readonly ISeasonsService _seasonsService = seasonsService;


    /// <summary>
    /// Добавляет студента в сезон.
    /// </summary>
    /// <param name="year">The year of the season</param>
    /// <param name="id">The ID of the student</param>
    [HttpPost("{year}/student/{id}")]
    public async Task<ActionResult<StudentInfo>> Create(int year, Guid id)
    {
        var season = await _seasonsService.Find(year, withCompanies: false, withStudents: true, checkOpen: true);
        return Ok(await _studentsService.Create(id, season));
    }


    /// <summary>
    /// Удаляет студента из сезона.
    /// </summary>
    /// <param name="year">The year of the season.</param>
    /// <param name="id">The ID of the student to delete.</param>
    [HttpDelete("{year}/student/{id}")]
    public async Task<ActionResult> Delete(int year, Guid id)
    {
        var season = await _seasonsService.Find(year, withCompanies: false, withStudents: true, checkOpen: true);
        await _studentsService.Delete(id, season);
        return Ok();
    }


    /// <summary>
    /// Получает всех студентов в сезоне.
    /// </summary>
    /// <param name="year">The year for which to retrieve the students.</param>
    /// <returns>An <see cref="ActionResult"/> containing the students for the specified year.</returns>
    [HttpGet("{year}/students")]
    public async Task<ActionResult> GetStudents(int year)
    {
        //TODO probably should be changed into separate query for student service
        var season = await _seasonsService.Find(year, withCompanies: true, withStudents: true);
        return Ok(_studentsService.ConvertToStudentsInfo(season.Students));
    }
}