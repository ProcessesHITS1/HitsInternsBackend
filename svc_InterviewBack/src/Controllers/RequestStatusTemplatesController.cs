using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[ApiController]
[Authorize]
public class RequestStatusTemplatesController(IRequestStatusTemplateService requestStatusService):ControllerBase
{
    //GET ALL STATUSES IN SEASON
    //CREATE/ADD STATUS IN SEASON
    //REMOVE STATUS FROM SEASON<IF IT IS NOWHERE USED
    
    /// <summary>
    /// Получает все допустимые статусы запроса в сезоне
    /// </summary>
    [HttpGet("season/{year}/request_statuses")] //TODO:permission admin
    public async Task<ActionResult<List<RequestStatusTemplateData>>> GetRequestStatusesInSeason(int year)
    {
        return Ok(await requestStatusService.GetRequestStatusesInSeason(year));
    }

    /// <summary>
    /// Создать статус в сезоне
    /// </summary>
    [HttpPost("season/{year}/request_status/{statusName}")] //TODO:permission admin
    public async Task<ActionResult> CreateRequestStatusInSeason(int year, string statusName)
    {
        await requestStatusService.CreateRequestStatusInSeason(year, statusName);
        return Ok();
    }
    
    //Todo: add remove StatusTemplate from season endpoint
}