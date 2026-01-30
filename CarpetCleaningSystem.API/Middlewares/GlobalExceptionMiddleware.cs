using CarpetCleaningSystem.Application.Exceptions; 
using System.Net;
using System.Text.Json;

namespace CarpetCleaningSystem.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = ex switch
            {
                CustomerNotFoundException => HttpStatusCode.NotFound,
                OrderNotFoundException => HttpStatusCode.NotFound,
                OrderItemNotFoundException => HttpStatusCode.NotFound,
                PhoneNumberAlreadyInUseException => HttpStatusCode.Conflict,
                OrderItemsLockedException => HttpStatusCode.Conflict,
                ArgumentException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var payload = new { message = ex.Message };

            return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}

