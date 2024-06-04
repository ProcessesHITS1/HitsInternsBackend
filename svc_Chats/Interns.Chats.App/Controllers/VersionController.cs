using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interns.Chats.App.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public string Version() => "1.0.0";

        [HttpGet("authenticated"), Authorize]
        public string VersionAuthenticated() => Version();
    }
}
