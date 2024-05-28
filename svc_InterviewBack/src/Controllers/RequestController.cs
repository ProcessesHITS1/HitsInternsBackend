using Microsoft.AspNetCore.Mvc;
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
    
    [HttpPost("season/{year}/student/{studentId}/position/{positionId}")] //student role
    public async Task<ActionResult> Create(int year,Guid studentId,Guid positionId)
    {
        return Ok(await requestService.CreateAsync(studentId,positionId));
    }

    
    [HttpPut("result_status")]// Student of request || staff
    public async Task<ActionResult> ChangeResultStatus()
    {
        //Check for Identity
        //Change Statue
        return Ok();
    }
    
    [HttpPut("request_status")] // Student of request
    public async Task<ActionResult> ChangeRequestStatus()
    {
        //Check for Identity
        //Change Status
        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete()
    {
        return Ok();
    }
    
    
}