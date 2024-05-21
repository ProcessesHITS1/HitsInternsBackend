using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[Route("/api/season")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ICompaniesService _companiesService;
    private readonly ISeasonsService _seasonsService;
    public CompaniesController(ICompaniesService companiesService, ISeasonsService seasonsService)
    {
        _companiesService = companiesService;
        _seasonsService = seasonsService;
    }

    [HttpPost("{year}/company/{id}")]
    public async Task<ActionResult> Create(int year, Guid id)
    {
        var season = await _seasonsService.Find(year);
        return Ok(await _companiesService.Create(id, season));
    }


    [HttpDelete("{year}/company/{id}")]
    public async Task<ActionResult> Delete(int year, Guid id)
    {
        var season = await _seasonsService.Find(year);
        await _companiesService.Delete(id, season);
        return Ok();
    }
}