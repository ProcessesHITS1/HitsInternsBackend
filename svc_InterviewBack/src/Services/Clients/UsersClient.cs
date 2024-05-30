using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using svc_InterviewBack.Utils;
using System.Text.Json.Serialization;


namespace svc_InterviewBack.Services.Clients;



public class UsersClient(HttpClient httpClient, AuthClient authClient, ILogger<AuthClient> logger, IMemoryCache cache, IConfiguration config)
{
    private readonly IConfiguration _config = config;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<User> GetUser(Guid id)
    {
        await authClient.TryAuthorize();
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/users/{id}/info");

        // add jwt token to headers
        var token = cache.Get("ClientToken");
        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await httpClient.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new NotFoundException($"User with id {id} not found");
        }
        response.EnsureSuccessStatusCode();
        logger.LogInformation("Got OK(200) response from users service");
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<User>(responseContent, _jsonOptions)!;
    }

    // Models
    public record User
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Patronymic { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Sex { get; set; }
        public required Group Group { get; set; }
        public required List<Role> Roles { get; set; }
    }

    public record Group
    {
        public Guid Id { get; set; }
        public required int Number { get; set; }
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        ROLE_STUDENT,
        ROLE_SCHOOL_REPRESENTATIVE,
        ROLE_ADMIN
    }

}