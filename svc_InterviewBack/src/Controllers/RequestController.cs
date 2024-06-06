using Microsoft.AspNetCore.Mvc;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Controllers;

[ApiController]
[Route("/api/request")]
public class RequestController(IRequestService requestService) : ControllerBase
{
    [HttpGet]
    //Authorized - Staff
    public async Task<ActionResult> Get()
    {
        return Ok();
    }

    //Authorized - student's history || Staff
    [HttpGet("{id}")]
    public async Task<ActionResult> GetRequestsHistory()
    {
        return Ok();
    }

    [HttpPost("student/{studentId}/position/{positionId}")] //student role
    public async Task<ActionResult> Create(Guid studentId, Guid positionId)
    {
        return Ok(await requestService.CreateAsync(studentId, positionId));
    }

    
    [HttpPut("{requestId}/result_status/{resultStatus}")]
    public async Task<ActionResult> UpdateResultStatus(Guid requestId, ResultStatus resultStatus)
    {
        return Ok(await requestService.UpdateResultStatus());
    }

    [HttpPut("{requestId}/request_status/{requestStatus}")] 
    public async Task<ActionResult> UpdateRequestStatus(Guid requestId,RequestStatus requestStatus)
    {
        return Ok(await requestService.UpdateRequestStatus());
    }
    
}