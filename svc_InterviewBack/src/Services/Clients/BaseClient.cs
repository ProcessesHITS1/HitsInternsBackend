using System.Text.Json;

namespace svc_InterviewBack.Services.Clients;


public static class ClientHelper
{
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public static async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions)!;
    }
}

