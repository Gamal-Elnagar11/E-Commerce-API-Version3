using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_Commerce_API.Filters
{
    public class GlobalExceptionFilter : Attribute, IExceptionFilter
    {
            public void OnException(ExceptionContext context)
            {
                var exception = context.Exception;
                int statusCode;
                string message;

                 switch (exception)
                {
                    case KeyNotFoundException:  
                        statusCode = StatusCodes.Status404NotFound;
                        message = exception.Message;
                        break;

                    case UnauthorizedAccessException: 
                        statusCode = StatusCodes.Status403Forbidden;
                        message = exception.Message;
                        break;

                    case ArgumentException: 
                        statusCode = StatusCodes.Status400BadRequest;
                        message = exception.Message;
                        break;

                    default:  
                        statusCode = StatusCodes.Status500InternalServerError;
                        message = "Internal Pronblem...   Plese Try again";
                        
                        break;
                }

                context.Result = new ObjectResult(new
                {
                    success = false,
                    error = message,
                    // Detail = exception.Message // فعلها وقت الـ Debug فقط
                })
                {
                    StatusCode = statusCode
                };

                context.ExceptionHandled = true;
            }
        
    }
}
