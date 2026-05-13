using Microsoft.AspNetCore.Http.Extensions;
using Mscc.GenerativeAI.Types;

namespace E_Commerce_API.Middelwares
{
    public class LogginMiddelware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogginMiddelware> _logger;

        public LogginMiddelware(RequestDelegate next, ILogger<LogginMiddelware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            _logger.LogInformation("Incoming Request: {Method} {Path}",
                request.Method, request.Path);

            var startTime = DateTime.Now;

            await _next(context);

            var duration = DateTime.Now - startTime;
            var response = context.Response;
            _logger.LogInformation("End Request: {StatusCode} | Duration: {Duration}ms",
                response.StatusCode, duration.TotalMilliseconds);
        }



        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

             var statusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

             var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Status = statusCode,
                Title = "An error occurred while processing your request.",
                Detail = exception.Message,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1", // لينك بيشرح نوع الخطأ
                Instance = context.Request.Path // المسار اللي حصل فيه المشكلة
            };

             problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            return context.Response.WriteAsJsonAsync(problemDetails);
        }



    }
}
