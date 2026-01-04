using Microsoft.AspNetCore.Diagnostics;
using Restaurant.Domain.Exceptions;
using Restaurant.Shared.DTO;
using System.Net;
using System.Text.Json;

namespace Restaurant.API.Middleware
{
    public class BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not RestaurantException or RestaurantServerException)
            {
                return false;
            }

            logger.LogInformation("Handling BadRequestException: {Message}", exception.Message);
            var content = exception switch
            {
                RestaurantServerException serverException => CreateErrorResponse(serverException),
                RestaurantException restaurantException => CreateErrorResponse(restaurantException),
                _ => throw new InvalidOperationException("Unhandled exception type.")
            };

            await HandleResponse(httpContext, content);
            return true;
        }

        private static async Task HandleResponse(HttpContext context, object errorResponse)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }

        private object CreateErrorResponse(RestaurantException exception)
        {
            return new ApiErrorResponse
            {
                Error = new ApiError
                {
                    Message = exception.Message,
                    Context = exception.Context
                }
            };
        }

        private object CreateErrorResponse(RestaurantServerException exception)
        {
            return new ApiErrorResponse
            {
                Error = new ApiError
                {
                    Message = exception.Message,
                    Context = exception.Context
                }
            };
        }
    }
}
