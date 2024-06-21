using System.ComponentModel.DataAnnotations;
using Interns.Common;
using Interns.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Controllers;

[ApiController]
[Authorize]
[Route("api/position")]
public class PositionsController(IPositionService positionService) : ControllerBase
{
    /// <summary>
    /// Создает новую позицию в компании в сезоне.
    /// </summary>
    /// <param name="positionData">The data for the position.</param>
    /// <returns>The created position.</returns>
    [HttpPost]
    public async Task<ActionResult<PositionInfo>> Create(PositionCreation positionData)
    {
        return Ok(await positionService.Create(positionData));
    }

    /// <summary>
    /// Удаляет позицию по её ID.
    /// </summary>
    /// <param name="id">The ID of the position to delete.</param>
    /// <returns>An empty response if the position is deleted successfully.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await positionService.Delete(id);
        return Ok();
    }

    /// <summary>
    /// Ищет позиции по компаниям, сезону и строке. Возвращает результаты по страницам.
    /// </summary>
    /// <param name="year">The year of the season.</param>
    /// <param name="companyIds">The IDs of the companies associated with the positions.</param>
    /// <param name="query">The search query.</param>
    /// <param name="page">The page number for pagination.</param>
    /// <returns>The search results.</returns>
    [HttpGet("search")]
    public async Task<ActionResult<PaginatedItems<PositionInfo>>> Search(
        int year,
        [FromQuery(Name = "companies")] List<Guid> companyIds,
        [FromQuery(Name = "q")] string query = "",
        int page = 1)
    {
        var queryModel = new PositionQuery
        {
            Query = query,
            CompanyIds = companyIds,
            SeasonYear = year
        };
        if (!ValidatePositionQuery(queryModel, out string errorMessage))
        {
            throw new BadRequestException(errorMessage);
        }
        return Ok(await positionService.Search(queryModel, page));
    }
    /// <summary>
    /// Обновление позиции.
    /// </summary>
    /// <param name="positionId">The ID of the position to update.</param>
    /// /// <param name="positionData">Updated position data.</param>
    [HttpPut("{positionId}")]
    public async Task<ActionResult<PositionUpdate>> PositionUpdate(Guid positionId, PositionUpdate positionData)
    {
        //Update Position Info
        return Ok(await positionService.Update(positionId, positionData));
    }
    

    private static bool ValidatePositionQuery(PositionQuery query, out string errorMessage)
    {
        var validationContext = new ValidationContext(query);
        var validationResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(query, validationContext, validationResults, true))
        {
            errorMessage = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
            return false;
        }
        errorMessage = "";
        return true;
    }

}