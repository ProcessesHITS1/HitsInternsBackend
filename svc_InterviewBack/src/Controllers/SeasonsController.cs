using Interns.Auth.Attributes.HasRole;
using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[Route("/api/season")]
[ApiController]
[CalledByStaff]
public class SeasonsController(ISeasonsService seasonsService, IStudentsService studentsService, ICompaniesService companiesService) : ControllerBase
{

    /// <summary>
    /// Получает все сезоны.
    /// </summary>
    /// <returns>A list of all seasons.</returns>
    [HttpGet("/api/seasons")]
    public async Task<ActionResult<List<Season>>> GetAll()
    {
        return Ok(await seasonsService.GetAll());
    }

    /// <summary>
    /// Получает информацию о сезоне.
    /// </summary>
    /// <param name="year">The year of the season to retrieve.</param>
    /// <returns>An ActionResult containing the retrieved Season object.</returns>
    [HttpGet("{year}/info")]
    public async Task<ActionResult<Season>> Get(int year)
    {
        return Ok(await seasonsService.Get(year));
    }

    /// <summary>
    /// ЭНДПОИНТ РАБОТАЕТ, НО ЛУЧШЕ ИСПОЛЬЗОВАТЬ ОТДЕЛЬНЫЕ ДЛЯ СТУДЕНТОВ И КОМПАНИЙ. Получает детали о сезоне, включая компании и студентов в нем.
    /// </summary>
    /// <param name="year">The year of the season to retrieve.</param>
    /// <returns>The season with the specified year.</returns>
    [Obsolete("Use separate endpoints for companies and students instead")]
    [HttpGet("{year}")]
    public async Task<ActionResult<SeasonDetails>> GetDetails(int year)
    {
        var season = await seasonsService.Find(year);
        return Ok(new SeasonDetails
        {
            Season = await seasonsService.Get(year),
            Companies = await companiesService.GetAll(year),
            Students = studentsService.ConvertToStudentsInfo(season.Students)
        });
    }

    /// <summary>
    /// Создает новый сезон.
    /// </summary>
    /// <param name="seasonData">The data for the new season.</param>
    /// <returns>The created season.</returns>
    [HttpPost]
    public async Task<ActionResult<Season>> Create(SeasonData seasonData)
    {
        return Ok(await seasonsService.Create(seasonData));
    }

    /// <summary>
    /// Обновляет сезон.
    /// </summary>
    /// <param name="year">The year of the season to update.</param>
    /// <param name="seasonData">The updated season data.</param>
    /// <returns>The updated season.</returns>
    [HttpPut("{year}")]
    public async Task<ActionResult<Season>> Update(int year, SeasonData seasonData)
    {
        return Ok(await seasonsService.Update(year, seasonData));
    }

    /// <summary>
    /// Удаляет сезон.
    /// </summary>
    /// <param name="year">The year of the season to delete.</param>
    /// <returns>An empty response.</returns>
    [HttpDelete("{year}")]
    public async Task<ActionResult> Delete(int year)
    {
        await seasonsService.Delete(year);
        return Ok();
    }

    /// <summary>
    /// Завершает текущий сезон. После завершения в нем нельзя создавать заявки и добавлять компании
    /// </summary>
    /// <param name="year">The year of the season to close.</param>
    /// <returns>An <see cref="ActionResult"/> representing the result of the operation.</returns>
    [HttpPost("{year}/close")]
    public async Task<ActionResult> Close(int year)
    {
        await seasonsService.Close(year);
        return Ok();
    }
}