using System.Security.Claims;
using E_Commerce_API.DTO.FeedBackDTO;
using E_Commerce_API.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mscc.GenerativeAI;

namespace E_Commerce_API.Controllers
{
    public static class FeedBack_MinimalAPI
    {

        public static void FeedbackMinimal(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/Feedback").WithTags("FeedBack")
                .RequireAuthorization(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                       .RequireAuthenticatedUser()
                       .Build());


            group.MapGet("/", async (IFeedbackService service) =>
            {
                var result = await service.GetAllFeedback();
                return Results.Ok(result);
            })
                .RequireAuthorization("Admin");


            group.MapPost("/", async (FeedDTO feedDto, HttpContext context, IFeedbackService service) =>
            {
                 var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = context.User.FindFirst(ClaimTypes.Name)?.Value;

                if (username == null || userId == null)
                    return Results.Unauthorized();

                await service.AddFeedback(feedDto.Rating, userId, feedDto.Comment, username);

                return Results.Ok("Feedback Added Success");
            })
                .RequireAuthorization("UserOrAdmin");



            group.MapDelete("/{id}", async (int id, IFeedbackService service) =>
            {
                try
                {

                var result = await service.DeleteFeedback(id);
                return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    return Results.NotFound("here");
                }
            })
                .RequireAuthorization("UserOrAdmin");


            

        }
    }
}

//app.UseExceptionHandler(appError =>
//{
//    appError.Run(async context =>
//    {
//        context.Response.ContentType = "application/problem+json"; // Content type الرسمي
//        var error = context.Features.Get<IExceptionHandlerFeature>();

//        if (error != null)
//        {
//            var status = error.Error switch
//            {
//                KeyNotFoundException => 404,
//                UnauthorizedAccessException => 401,
//                _ => 500
//            };

//            context.Response.StatusCode = status;

//            var problem = new ProblemDetails
//            {
//                Status = status,
//                Title = status == 500 ? "Internal Server Error" : error.Error.Message,
//                Detail = error.Error.StackTrace,
//            };

//            await context.Response.WriteAsJsonAsync(problem);
//        }
//    });
//});
