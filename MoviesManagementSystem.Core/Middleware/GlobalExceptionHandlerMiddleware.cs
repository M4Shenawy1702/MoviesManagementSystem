using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesManagementSystem.Core.Errors;
using System.Net;
using System.Text.Json;

namespace MoviesManagementSystem.Core.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed with the request pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // Catch any unhandled exceptions
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            // Log exception for debugging
            Console.WriteLine($"Exception Type: {exception.GetType().Name}");
            Console.WriteLine($"Message: {exception.Message}");

            var (statusCode, problemDetails) = exception switch
            {
                ServiceException serviceException => (serviceException.StatusCode, new ProblemDetails
                {
                    Status = serviceException.StatusCode,
                    Title = "Resource Not Found",
                    Detail = serviceException.Message
                }),
                _ => ((int)HttpStatusCode.InternalServerError, new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Detail = exception.Message
                })
            };

            response.StatusCode = statusCode;
            var result = JsonSerializer.Serialize(problemDetails);
            return response.WriteAsync(result);
        }
    }


}
