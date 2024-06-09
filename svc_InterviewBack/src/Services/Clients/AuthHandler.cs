using Microsoft.Extensions.Caching.Memory;


namespace svc_InterviewBack.Services.Clients;

public class AuthHandler(AuthClient authClient, IMemoryCache cache) : DelegatingHandler
{
    private readonly AuthClient _authClient = authClient;
    private readonly IMemoryCache _cache = cache;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await _authClient.TryAuthorize();

        // Add JWT token to headers
        var token = _cache.Get<string>("ClientToken");
        if (token != null)
        {
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
