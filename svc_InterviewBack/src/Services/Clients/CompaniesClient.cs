using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services.Clients;


public class CompaniesClient(HttpClient httpClient, AuthClient authClient, ILogger<CompaniesClient> logger, IMemoryCache cache)
{
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<CompanyData> Get(Guid id)
    {
        await authClient.TryAuthorize();
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/companies/{id}");

        // add jwt token to headers
        var token = cache.Get("ClientToken");
        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await httpClient.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new NotFoundException($"Company with id {id} not found");
        }
        response.EnsureSuccessStatusCode();
        logger.LogInformation("Got OK(200) response from companies service");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CompanyData>(responseContent, _jsonOptions)!;
    }

    // Models
    public record CompanyData
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string CuratorId { get; set; }
        public required List<string> Contacts { get; set; }
    }
}

