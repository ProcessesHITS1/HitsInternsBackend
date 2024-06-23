using Interns.Auth.Attributes.HasRole;
using Interns.Auth.Extensions;
using Interns.Common.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Models;
using svc_InterviewBack.Services;
using static Interns.Auth.Attributes.HasRole.HasRoleAttribute;

namespace svc_InterviewBack.Controllers;

[ApiController]
[Authorize]
[Route("/api/request")]
public class RequestController(IRequestService requestService) : ControllerBase
{
    /// <summary>
    /// Получает информацию о запросах стажировку. Endpoint для администратора.
    /// </summary>
    /// <param name="companyIds">фильтрация по компаниям</param>
    /// <param name="seasonYears">Фильтрация по сезонам</param>
    /// <param name="studentIds">фильтрация по студентам.</param>
    /// <param name="includeHistory">включать всю историю статусов, или включать только текущий статус запроса.</param>
    /// <returns>Пагинированные запросы.</returns>
    [HttpGet]
    //TODO: фильтрация по позициям?
    [CalledByStaff]
    public async Task<ActionResult<PaginatedItems<RequestData>>> GetRequests(
        [FromQuery(Name = "seasons")] List<int> seasonYears,
        [FromQuery(Name = "companyIds")] List<Guid> companyIds,
        [FromQuery(Name = "studentIds")] List<Guid> studentIds,
        int page = 1,
        int pageSize = 10,
        bool includeHistory = false
    )
    {
        var requestsQuery = new RequestQuery
        {
            SeasonYears = seasonYears,
            StudentIds = studentIds,
            CompanyIds = companyIds,
            IncludeHistory = includeHistory
        };
        return Ok(await requestService.GetRequests(requestsQuery, page, pageSize));
    }

    /// <summary>
    /// Получает информацию о запросах стажировку. Endpoint для студента.
    /// </summary>
    /// <param name="seasonYears">фильтрация по сезонам</param>
    /// <param name="companyIds">фильтрация по компаниям</param>
    /// <param name="includeHistory">включать всю историю статусов, или включать только текущий статус запроса.</param>
    /// <returns>Пагинированные запросы.</returns>
    [HttpGet("my")]
    [CalledByStudent]
    public async Task<ActionResult<PaginatedItems<RequestData>>> GetStudentRequests(
        [FromQuery(Name = "seasons")] List<int> seasonYears,
        [FromQuery(Name = "companyIds")] List<Guid> companyIds,
        int page = 1,
        int pageSize = 10,
        bool includeHistory = false)
    {
        var studentId = User.GetId();
        var requestsQuery = new RequestQuery
        {
            StudentIds = [studentId],
            CompanyIds = companyIds,
            SeasonYears = seasonYears,
            IncludeHistory = includeHistory
        };
        return Ok(await requestService.GetRequests(requestsQuery, page, pageSize));
    }

    [HttpGet("{requestId}")]
    public async Task<ActionResult<RequestData>> GetRequest(Guid requestId)
    {
        var isStudent = User.IsInRole(UserRoles.STUDENT);
        var userId = User.GetId();

        return Ok(await requestService.GetRequest(requestId, userId, isStudent));
    }


    /// <summary>
    /// Создать запрос с начальным статусом.
    /// </summary>
    [HttpPost("position/{positionId}/status/{requestStatusId}")]
    [CalledByStudent]
    public async Task<ActionResult<RequestDetails>> Create(Guid positionId, Guid requestStatusId)
    {
        var studentId = User.GetId();
        return Ok(await requestService.Create(studentId, positionId, requestStatusId));
    }

    /// <summary>
    /// Обновить результат запроса. Статусы:(Pending,Accepted,Rejected)
    /// </summary>
    [HttpPut("{requestId}/result_status")] //TODO: add checks for user's identity 
    public async Task<ActionResult> UpdateResultStatus(Guid requestId, RequestResultUpdate reqResult)
    {
        var userId = User.GetId();
        var isStudent = User.IsStudent();
        var isStaff = User.IsStaff();

        if (!isStaff) reqResult.SchoolResultStatus = null;
        if (!isStudent) reqResult.StudentResultStatus = null;
        
        return Ok(await requestService.UpdateResultStatus(requestId, userId, isStaff, reqResult));
    }

    /// <summary>
    /// Обновить статус запроса.
    /// </summary>
    [HttpPut("{requestId}/request_status/{requestStatusId}")] //TODO: add checks for user's identity 
    public async Task<ActionResult> UpdateRequestStatus(Guid requestId, Guid requestStatusId)
    {
        return Ok(await requestService.UpdateRequestStatus(requestId, requestStatusId));
    }
}