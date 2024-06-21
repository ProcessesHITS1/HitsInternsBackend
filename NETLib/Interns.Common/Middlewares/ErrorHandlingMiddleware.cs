using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Interns.Common.Middlewares
{
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }

    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                switch (ex)
                {
                    case NotFoundException:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;
                    case AccessDeniedException:
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        break;
                    case BadRequestException:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;
                    case MicroserviceException or HttpRequestException:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        _logger.LogWarning(ex.Message);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Microservice error. " + ex.Message }));
                        return;
                    default:
                        _logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Internal server error" }));
                        return;
                }
                _logger.LogInformation(ex.Message);
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
            }
        }
    }
}
