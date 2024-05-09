using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[Route("/api/season")]
[ApiController]
public class SeasonsController : ControllerBase
{
    private readonly ISeasonsService _seasonsService;
    private readonly ILogger<SeasonsController> _logger;
    public SeasonsController(ISeasonsService seasonsService, ILogger<SeasonsController> logger)
    {
        _seasonsService = seasonsService;
        _logger = logger;
    }

    [HttpGet("/api/seasons")]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _seasonsService.GetAll());
    }

    [HttpGet("{year}")]
    public async Task<ActionResult> Get(int year)
    {
        return Ok(await _seasonsService.Get(year));
    }

    [HttpPost]
    public async Task<ActionResult> Create(SeasonData seasonData)
    {
        return Ok(await _seasonsService.Create(seasonData));
    }

    [HttpPut("{year}")]
    public async Task<ActionResult> Update(int year, SeasonData seasonData)
    {
        return Ok(await _seasonsService.Update(year, seasonData));
    }

    [HttpDelete("{year}")]
    public async Task<ActionResult> Delete(int year)
    {
        await _seasonsService.Delete(year);
        return Ok();
    }
}