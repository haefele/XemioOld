using Microsoft.AspNetCore.Mvc;

namespace Xemio.Server.Infrastructure.Controllers
{
    [Route("api/debug")]
    public class DebugController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hoi";
        }
    }
}