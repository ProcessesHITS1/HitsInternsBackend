using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[ApiController]
[Route("api/position")]
public class PositionsController(IPositionService positionService) : ControllerBase
{
    [HttpPost("{companyId}")]
    public async Task<ActionResult> Create(Guid companyId, PositionData positionData)
    {
        return Ok(await positionService.Create(companyId, positionData));
    }
    
    
}