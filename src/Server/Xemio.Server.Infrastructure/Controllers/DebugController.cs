using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xemio.Server.Infrastructure.Entities.Notes;

namespace Xemio.Server.Infrastructure.Controllers
{
    [Route("debug")]
    public class DebugController : ControllerBase
    {
    }
}