using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services.Clients;


public class CompaniesClient(HttpClient httpClient)
{
    public async Task<CompanyData> Get(Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/companies/{id}");

        var response = await httpClient.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new NotFoundException($"Company with id {id} not found");
        }
        return await ClientHelper.DeserializeResponse<CompanyData>(response);
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

