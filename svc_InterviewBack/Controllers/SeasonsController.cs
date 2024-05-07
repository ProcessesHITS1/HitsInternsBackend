using Microsoft.AspNetCore.Mvc;

namespace svc_InterviewBack.Controllers;

[Route("/api/season")]
[ApiController]
public class SeasonsController : ControllerBase
{
    public SeasonsController()
    {

    }

    [HttpGet("/api/seasons")]
    public ActionResult GetAll()
    {
        return Ok();
    }

    [HttpGet("{year}")]
    public ActionResult Get(int year)
    {
        return Ok();
    }
}