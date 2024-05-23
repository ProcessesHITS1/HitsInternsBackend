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
    /// <param name="id">The ID of the company</param>
    [HttpPost("{year}/student/{id}")]
    public async Task<ActionResult> Create(int year, Guid id)
    {
        var season = await _seasonsService.Find(year, withCompanies: false, withStudents: true);
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
        var season = await _seasonsService.Find(year, withCompanies: false, withStudents: true);
        await _studentsService.Delete(id, season);
        return Ok();
    }
}