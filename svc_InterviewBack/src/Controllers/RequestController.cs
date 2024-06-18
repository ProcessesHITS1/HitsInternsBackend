using Interns.Auth.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[ApiController]
[Authorize]
[Route("/api/request")]
public class RequestController(IRequestService requestService) : ControllerBase
{
    
    /// <summary>
    /// Получает информацию о запросах стажировку. Endpoint для администратора.
    /// </summary>
    /// <param name="companyIds">Пока что не работает.</param>
    /// <param name="studentIds">фильтрация по студентам.</param>
    /// <param name="requestIds">фильтрация по запросам.</param>
    /// <param name="includeHistory">включать всю историю статусов, или включать только текущий статус запроса.</param>
    /// <returns>Пагинированные запросы.</returns>
    [HttpGet]
    //TODO:Set Role 
    //Authorized - Staff
    public async Task<ActionResult> GetRequests(
        [FromQuery(Name = "companies")] List<Guid>? companyIds,
        [FromQuery(Name = "students")] List<Guid>? studentIds,
        [FromQuery(Name = "requests")] List<Guid>? requestIds,
        int page = 1,
        int pageSize=10,
        bool includeHistory=false 
        )
    {
        var requestsQuery = new RequestQuery
        {
            RequestIds = requestIds,
            StudentIds = studentIds,
            CompanyIds = companyIds,
            IncludeHistory = includeHistory
        };
        return Ok(await requestService.GetRequests(true,requestsQuery, page, pageSize));
    }

    /// <summary>
    /// Получает информацию о запросах стажировку. Endpoint для студента.
    /// </summary>
    /// <param name="companyIds">Пока что не работает.</param>
    /// <param name="requestIds">фильтрация по запросам.</param>
    /// <param name="includeHistory">включать всю историю статусов, или включать только текущий статус запроса.</param>
    /// <returns>Пагинированные запросы.</returns>
    [HttpGet("student")]
    public async Task<ActionResult> GetStudentRequests( 
        [FromQuery(Name = "companies")] List<Guid>? companyIds,
        [FromQuery(Name = "requests")] List<Guid>? requestIds,
        int page = 1,
        int pageSize=10,
        bool includeHistory=false)
    {
        var studentId = User.GetId();
        var requestsQuery = new RequestQuery
        {
            StudentIds = new List<Guid>{studentId},
            RequestIds = requestIds,
            CompanyIds = companyIds,
            IncludeHistory = includeHistory
        };
        return Ok(await requestService.GetRequests(true,requestsQuery, page, pageSize));
    }

    [HttpPost("position/{positionId}/status/{statusName}")] //student role
    public async Task<ActionResult<RequestDetails>> Create(Guid positionId,string statusName)
    {
        var studentId = User.GetId();
        return Ok(await requestService.CreateAsync(studentId, positionId,statusName));
    }

    /// <summary>
    /// Обновить результат запроса. Статусы:(Pending,Accepted,Rejected)
    /// </summary>
    
    [HttpPut("{requestId}/result_status")] //TODO: add checks for user's identity 
    public async Task<ActionResult> UpdateResultStatus(Guid requestId, RequestResultData reqResult)
    {
        return Ok(await requestService.UpdateResultStatus(requestId,reqResult));
    }

    [HttpPut("{requestId}/request_status/{requestStatus}")] //TODO: add checks for user's identity 
    public async Task<ActionResult> UpdateRequestStatus(Guid requestId,string requestStatus)
    {
        return Ok(await requestService.UpdateRequestStatus(requestId,requestStatus));
    }
    
    /// <summary>
    /// Получает все допустимые статусы запроса в сезоне
    /// </summary>
    [HttpGet("season/{year}/request_statuses")]
    public async Task<ActionResult> GetRequestStatusesInSeason(int year)
    {
        return Ok(await requestService.GetRequestStatusesInSeason(year));
    }
    /// <summary>
    /// Создать статус в сезоне
    /// </summary>
    [HttpPost("season/{year}/request_status/{statusName}")]
    public async Task<ActionResult> CreateRequestStatusInSeason(int year, string statusName)
    {
        await requestService.CreateRequestStatusInSeason(year, statusName);
        return Ok();
    }
}