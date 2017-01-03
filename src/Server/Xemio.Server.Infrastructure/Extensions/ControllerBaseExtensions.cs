using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raven.Abstractions.Data;
using Xemio.Server.Infrastructure.Entities;

namespace Xemio.Server.Infrastructure.Extensions
{

    public static class ControllerBaseExtensions
    {
        public static StatusCodeResult Conflict(this ControllerBase self)
        {
            return self.StatusCode(StatusCodes.Status409Conflict);
        }
    }
}
