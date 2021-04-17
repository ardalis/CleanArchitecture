using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clean.Architecture.Web.Filters
{
    /// <summary>
    /// This filter is no longer needed since [ApiController] provides this automatically for APIs.
    /// Both the BaseApiController and all ApiEndpoints in this sample use [ApiController].
    /// This file is included to show how and where additional custom filters would be added to your Web project.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
