using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Xemio.Server.Infrastructure.Filters
{
    public class RequiredParameterFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null)
            {
                var parameters = descriptor.MethodInfo.GetParameters();

                foreach (var parameter in parameters)
                {
                    foreach (var attribute in parameter.CustomAttributes)
                    {
                        var attributeInstance = parameter.GetCustomAttribute(attribute.AttributeType);
                        if (attributeInstance is RequiredAttribute requiredAttribute)
                        {
                            var argument = context.ActionArguments[parameter.Name];

                            if (requiredAttribute.IsValid(argument) == false)
                                context.Result = new BadRequestResult();
                        }
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}