using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace svc_InterviewBack.Services.Clients;


public class BaseClient(HttpClient httpClient, AuthClient authClient, IMemoryCache cache)
{
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<HttpResponseMessage> SendWithAuth(HttpRequestMessage message)
    {
        await authClient.TryAuthorize();

        // add jwt token to headers
        var token = cache.Get("ClientToken");
        message.Headers.Add("Authorization", $"Bearer {token}");

        var response = await httpClient.SendAsync(message);
        return response;
    }

    public async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions)!;
    }
}

