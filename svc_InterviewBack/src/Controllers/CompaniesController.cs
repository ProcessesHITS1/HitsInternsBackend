using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[Route("/api/season")]
[ApiController]
public class CompaniesController(ICompaniesService companiesService, ISeasonsService seasonsService) : ControllerBase
{
    private readonly ICompaniesService _companiesService = companiesService;
    private readonly ISeasonsService _seasonsService = seasonsService;


    /// <summary>
    /// Добавляет компанию в сезон.
    /// </summary>
    /// <param name="year">The year of the season.</param>
    /// <param name="id">The ID of the company.</param>
    [HttpPost("{year}/company/{id}")]
    public async Task<ActionResult> Create(int year, Guid id)
    {
        var season = await _seasonsService.Find(year, withCompanies: true, withStudents: false);
        return Ok(await _companiesService.Create(id, season));
    }


    /// <summary>
    /// Удаляет компанию из сезона.
    /// </summary>
    /// <param name="year">The year of the season.</param>
    /// <param name="id">The ID of the company to delete.</param>
    [HttpDelete("{year}/company/{id}")]
    public async Task<ActionResult> Delete(int year, Guid id)
    {
        var season = await _seasonsService.Find(year, withCompanies: true, withStudents: false);
        await _companiesService.Delete(id, season);
        return Ok();
    }
}