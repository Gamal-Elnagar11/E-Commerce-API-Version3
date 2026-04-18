using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace E_Commerce_API.Middelwares
{
    public class GlobalErrorHandling(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (StatusCode, title) = exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest,"Bad Request"),
                ArgumentException => (StatusCodes.Status400BadRequest,"Bad Request"),
                KeyNotFoundException => (StatusCodes.Status404NotFound,"Resource Not Found"),
                UnauthorizedAccessException => (StatusCodes.Status403Forbidden,"Access Denied"),
                _ => (StatusCodes.Status500InternalServerError,"Internal Server Error")
            };

            httpContext.Response.StatusCode = StatusCode;

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Status = StatusCode,
                    Title = title,
                    Detail = exception.Message,
                    Type = exception.GetType().Name,
                    Instance = httpContext.Request.Path
                }
            });
        }
    }
}
