using System.Net;
using System.Text.Json;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Middlewares;

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
            if (ex is NotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else if (ex is BadRequestException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (ex is MicroserviceException or HttpRequestException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogWarning(ex.Message);
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Microservice error. " + ex.Message }));
                return;
            }
            else
            {
                _logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Internal server error" }));
                return;
            }
            _logger.LogInformation(ex.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
        }
    }
}