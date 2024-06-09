using System.Text;
using System.Text.Json;

namespace svc_InterviewBack.Services.Clients;


public static class ClientHelper
{
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions)!;
    }

    public static HttpRequestMessage SerializeRequest<T>(HttpMethod method, string uri, T content)
    {
        return new HttpRequestMessage(method, uri)
        {
            Content = new StringContent(JsonSerializer.Serialize(content, _jsonOptions), Encoding.UTF8, "application/json")
        };
    }
}

