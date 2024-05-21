using System.Text;
using System.Text.Json;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services.Clients;


public class CompaniesClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CompaniesClient> _logger;

    public CompaniesClient(HttpClient httpClient, ILogger<CompaniesClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CompanyData> Get(Guid id)
    {
        var content = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/companies", content);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new NotFoundException($"Company with id {id} not found");
        }
        response.EnsureSuccessStatusCode();
        _logger.LogInformation("Got OK(200) response from companies service");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CompanyData>(responseContent) ?? throw new Exception("Failed to deserialize response");
    }

    // Models
    public class CompanyData
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string CuratorId { get; set; }
        public required List<string> Contacts { get; set; }
        public required List<Guid> SeasonIds { get; set; }
    }
}

