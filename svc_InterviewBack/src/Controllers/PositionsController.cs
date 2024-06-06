using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[ApiController]
[Route("api/position")]
public class PositionsController(IPositionService positionService, ISeasonsService seasonsService) : ControllerBase
{
    [HttpPost("/api/season/{year}/company/{companyId}/position")]
    public async Task<ActionResult> Create(int year, Guid companyId, PositionData positionData)
    {
        var season = await seasonsService.Find(year, false, false);
        return Ok(await positionService.Create(season, companyId, positionData));
    }

    [HttpGet("search")]
    public async Task<ActionResult> Search(
        [FromQuery(Name = "companies")] List<Guid> companyIds,
        [FromQuery(Name = "q")] string query = "",
        int page = 1)
    {
        var queryModel = new PositionQuery
        {
            Query = query,
            CompanyIds = companyIds
        };
        return Ok(await positionService.Search(queryModel, page));
    }
    /// <summary>
    /// Обновление позиции.
    /// </summary>
    /// <param name="positionId">The ID of the position to update.</param>
    /// /// <param name="positionData">Updated position data.</param>
    [HttpPut("{positionId}")]
    public async Task<ActionResult> Update(Guid positionId, PositionData positionData)
    {
        //Update Position Info
        return Ok(await positionService.Update(positionId, positionData));
    }
    
}