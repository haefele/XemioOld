using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Xemio.Server.Infrastructure.Filters
{
    public class ConcurrencyExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            if (context.Exception is DbUpdateConcurrencyException)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status409Conflict);
            }
        }
    }
}