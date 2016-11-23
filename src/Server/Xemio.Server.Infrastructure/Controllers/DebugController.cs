using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Xemio.Server.Infrastructure.Controllers
{
    [Route("debug")]
    public class DebugController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public string Get()
        {
            return "Hoi";
        }
    }
}