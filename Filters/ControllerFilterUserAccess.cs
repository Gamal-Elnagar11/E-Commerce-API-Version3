using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace E_Commerce_API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ControllerFilterUserAccess : ActionFilterAttribute
    {
         
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ControllerFilterUserAccess>>();

            // check loggin 
            var user = context.HttpContext.User;
            if(user.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "must be logged in to access this point" });
                return;
            }

            var username = user.Identity.Name ?? "User";
            logger.LogInformation($"Welcome : ({username})");

            var watch = Stopwatch.StartNew();

            var execution = await next();

            watch.Stop();
            if( execution.Exception == null )
            {
              logger.LogInformation($"Success User ({username})  and finshed you operation in ({watch.ElapsedMilliseconds}ms)");
            }
            else
            {
                logger.LogError($"Failed User({username}) has problem and take time ({watch.ElapsedMilliseconds})");
            }

        }
    }
}
