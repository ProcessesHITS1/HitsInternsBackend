using Interns.Auth.Extensions;
using Interns.Common.Pagination;
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
    public async Task<ActionResult<PaginatedItems<RequestData>>> GetRequests(
        [FromQuery(Name = "companies")] List<Guid>? companyIds,
        [FromQuery(Name = "students")] List<Guid>? studentIds,
        [FromQuery(Name = "requests")] List<Guid>? requestIds,
        int page = 1,
        int pageSize = 10,
        bool includeHistory = false
    )
    {
        var requestsQuery = new RequestQuery
        {
            RequestIds = requestIds,
            StudentIds = studentIds,
            CompanyIds = companyIds,
            IncludeHistory = includeHistory
        };
        return Ok(await requestService.GetRequests(requestsQuery, page, pageSize));
    }

    /// <summary>
    /// Получает информацию о запросах стажировку. Endpoint для студента.
    /// </summary>
    /// <param name="companyIds">Пока что не работает.</param>
    /// <param name="requestIds">фильтрация по запросам.</param>
    /// <param name="includeHistory">включать всю историю статусов, или включать только текущий статус запроса.</param>
    /// <returns>Пагинированные запросы.</returns>
    [HttpGet("my")]
    public async Task<ActionResult<PaginatedItems<RequestData>>> GetStudentRequests(
        [FromQuery(Name = "companies")] List<Guid>? companyIds,
        [FromQuery(Name = "requests")] List<Guid>? requestIds,
        int page = 1,
        int pageSize = 10,
        bool includeHistory = false)
    {
        var studentId = User.GetId();
        var requestsQuery = new RequestQuery
        {
            StudentIds = [studentId],
            RequestIds = requestIds,
            CompanyIds = companyIds,
            IncludeHistory = includeHistory
        };
        return Ok(await requestService.GetRequests(requestsQuery, page, pageSize));
    }

    /// <summary>
    /// Создать запрос с начальным статусом.
    /// </summary>
    [HttpPost("position/{positionId}/status/{statusName}")] //student role
    public async Task<ActionResult<RequestDetails>> Create(Guid positionId, string statusName)
    {
        var studentId = User.GetId();
        return Ok(await requestService.Create(studentId, positionId, statusName));
    }

    /// <summary>
    /// Обновить результат запроса. Статусы:(Pending,Accepted,Rejected)
    /// </summary>
    [HttpPut("{requestId}/result_status")] //TODO: add checks for user's identity 
    public async Task<ActionResult> UpdateResultStatus(Guid requestId, RequestResultUpdate reqResult)
    {
        return Ok(await requestService.UpdateResultStatus(requestId, reqResult));
    }

    /// <summary>
    /// Обновить статус запроса.
    /// </summary>
    [HttpPut("{requestId}/request_status/{requestStatus}")] //TODO: add checks for user's identity 
    public async Task<ActionResult> UpdateRequestStatus(Guid requestId, string requestStatus)
    {
        return Ok(await requestService.UpdateRequestStatus(requestId, requestStatus));
    }


}