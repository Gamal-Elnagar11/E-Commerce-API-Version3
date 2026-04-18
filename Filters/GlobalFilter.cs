using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace E_Commerce_API.Filters
{
    public class GlobalFilter : IAsyncActionFilter
    {
        private readonly ILogger<GlobalFilter> logger;

        public GlobalFilter(ILogger<GlobalFilter> logger)
        {
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Validation for all EndPoint
            if(!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            // calculate time
            var time = Stopwatch.StartNew();
            var requestName = context.ActionDescriptor.DisplayName;
            logger.LogInformation($"who try to entry : {requestName}");

            var executedContxt =  await next();

            time.Stop();
            var timeByMilliseconds = time.ElapsedMilliseconds;
            if(executedContxt.Exception == null)
            {
                logger.LogInformation($"The Name : ({requestName})     is completed in ({timeByMilliseconds})ms ");

                // Add Time execute to header
                context.HttpContext.Response.Headers.Append("X-Execution-Time-Ms", timeByMilliseconds.ToString());
            }
            else
            {
                logger.LogError($"Name is :({requestName}) has problem after ({timeByMilliseconds})");
            }



        }
    }
}
