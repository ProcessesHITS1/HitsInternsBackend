using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[Route("/api/season")]
[ApiController]
public class SeasonsController(ISeasonsService seasonsService) : ControllerBase
{
    private readonly ISeasonsService _seasonsService = seasonsService;

    /// <summary>
    /// Получает все сезоны.
    /// </summary>
    /// <returns>A list of all seasons.</returns>
    [HttpGet("/api/seasons")]
    public async Task<ActionResult> GetAll()
    {
        return Ok(await _seasonsService.GetAll());
    }

    /// <summary>
    /// Получает детали о сезоне, включая компании и студентов в нем.
    /// </summary>
    /// <param name="year">The year of the season to retrieve.</param>
    /// <returns>The season with the specified year.</returns>
    [HttpGet("{year}")]
    public async Task<ActionResult> Get(int year)
    {
        return Ok(await _seasonsService.Get(year));
    }

    /// <summary>
    /// Создает новый сезон.
    /// </summary>
    /// <param name="seasonData">The data for the new season.</param>
    /// <returns>The created season.</returns>
    [HttpPost]
    public async Task<ActionResult> Create(SeasonData seasonData)
    {
        return Ok(await _seasonsService.Create(seasonData));
    }

    /// <summary>
    /// Обновляет сезон.
    /// </summary>
    /// <param name="year">The year of the season to update.</param>
    /// <param name="seasonData">The updated season data.</param>
    /// <returns>The updated season.</returns>
    [HttpPut("{year}")]
    public async Task<ActionResult> Update(int year, SeasonData seasonData)
    {
        return Ok(await _seasonsService.Update(year, seasonData));
    }

    /// <summary>
    /// Удаляет сезон.
    /// </summary>
    /// <param name="year">The year of the season to delete.</param>
    /// <returns>An empty response.</returns>
    [HttpDelete("{year}")]
    public async Task<ActionResult> Delete(int year)
    {
        await _seasonsService.Delete(year);
        return Ok();
    }
}