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

    //Authorized - student's history || Staff
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

    
    [HttpPut("{requestId}/result_status")]
    public async Task<ActionResult> UpdateResultStatus(Guid requestId, RequestResultData reqResult)
    {
        return Ok(await requestService.UpdateResultStatus(requestId,reqResult));
    }

    [HttpPut("{requestId}/request_status/{requestStatus}")] 
    public async Task<ActionResult> UpdateRequestStatus(Guid requestId,string requestStatus)
    {
        return Ok(await requestService.UpdateRequestStatus(requestId,requestStatus));
    }
    
    
    [HttpGet("season/{year}/request_statuses")]
    public async Task<ActionResult> GetRequestStatusesInSeason(int year)
    {
        return Ok(await requestService.GetRequestStatusesInSeason(year));
    }

    [HttpPost("season/{year}/request_status/{statusName}")]
    public async Task<ActionResult> CreateRequestStatusInSeason(int year, string statusName)
    {
        await requestService.CreateRequestStatusInSeason(year, statusName);
        return Ok();
    }
}