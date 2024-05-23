using Microsoft.Extensions.Caching.Memory;
using svc_InterviewBack.Services.Clients;

namespace svc_InterviewBack.Middlewares;

public static class AuthRefreshMiddlewareMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthRefreshMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthRefreshMiddleware>();
    }
}

public class AuthRefreshMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IMemoryCache cache)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;
    private readonly IMemoryCache _cache = cache;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (!_cache.TryGetValue("ClientToken", out string? token))
            {
                var client = context.RequestServices.GetRequiredService<AuthClient>();
                await client.Authorize();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);
        }
        finally
        {
            await _next(context);
        }
    }
}