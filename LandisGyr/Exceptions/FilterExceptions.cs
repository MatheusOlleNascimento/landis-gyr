using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LandisGyr.Exceptions
{
    public class FilterExceptions : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentNullException ane)
            {
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new ObjectResult(new { ErrorMessage = ane.Message ?? $"ConsumerUnit not found" });
            }

            else if (context.Exception is InvalidOperationException ioe)
            {
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new ObjectResult(new { ErrorMessage = ioe.Message ?? "This serial number is already in use" });
            }

            base.OnException(context);
        }
    }
}