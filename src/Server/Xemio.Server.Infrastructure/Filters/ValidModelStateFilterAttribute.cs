using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Xemio.Server.Infrastructure.Filters
{
    public class ValidModelStateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
                context.Result = new BadRequestObjectResult(context.ModelState);

            base.OnActionExecuting(context);
        }
    }
}