namespace svc_InterviewBack.Services.Clients;

public class AuthHandler(AuthClient authClient) : DelegatingHandler
{
    private readonly AuthClient _authClient = authClient;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authClient.GetAccessToken();
        if (token != null)
        {
            request.Headers.Add("Authorization", $"Bearer {token}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
