using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mscc.GenerativeAI;
using System.Security.Claims;

namespace E_Commerce_API.Filters
{
    public class ActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ActionFilter>>();

            var user = context.HttpContext.User;

            if(user.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedObjectResult(new { massage = "must be Admin to Access this point" });
                return;
            }


            bool isAdmin = user.IsInRole("Admin") ||
                                       user.HasClaim(ClaimTypes.Role, "Admin") ||
                                       user.HasClaim("role", "Admin");

            if (!isAdmin)
            {
                var username = user.Identity.Name ?? "Unknown";
                logger.LogWarning($" {username} -> This User Unable to Access this point");

                base.OnActionExecuting(context);


                context.Result = new ObjectResult(new { message = " this is allowed to Admin Only" })
                {
                    StatusCode = 403
                };
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
