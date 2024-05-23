using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;


namespace svc_InterviewBack.Services.Clients;



public class AuthClient(HttpClient httpClient, ILogger<AuthClient> logger, IMemoryCache cache, IConfiguration config)
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<AuthClient> _logger = logger;
    private readonly IMemoryCache _cache = cache;
    private readonly IConfiguration _config = config;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task Authorize()
    {
        var email = _config["AuthLogin"];
        var password = _config["AuthPassword"];
        var content = new StringContent(JsonSerializer.Serialize(new { email, password }), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/users/sign-in", content);
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<TokenData>(body, _jsonOptions)!.AccessToken;
        var info = TokenHelper.ParseToken(token);
        var lifetime = TokenHelper.GetTokenLifetime(info);
        _logger.LogInformation($"Got token with lifetime: {lifetime}");
        _cache.Set("ClientToken", token, lifetime);
    }

    // Models
    public record TokenData
    {
        public required string AccessToken { get; set; }
    }
}


public static class TokenHelper
{
    public static JwtSecurityToken ParseToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        return jsonToken!;
    }

    public static TimeSpan GetTokenLifetime(JwtSecurityToken token)
    {
        var exp = token.Claims.First(claim => claim.Type == "exp");
        var iat = token.Claims.First(claim => claim.Type == "iat");
        return TimeSpan.FromSeconds(long.Parse(exp.Value) - long.Parse(iat.Value));
    }
}