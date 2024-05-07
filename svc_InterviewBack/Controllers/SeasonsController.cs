using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllers;

[Route("/api/season")]
[ApiController]
public class SeasonsController : ControllerBase
{
    public SeasonsController()
    {

    }

    [HttpGet("{year}")]
    public ActionResult Get(int year)
    {
        return Ok();
    }
}