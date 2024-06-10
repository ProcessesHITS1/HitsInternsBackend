using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using svc_InterviewBack.Utils;


namespace svc_InterviewBack.Services.Clients;



public class AuthClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthClient> _logger;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _config;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public AuthClient(HttpClient httpClient, ILogger<AuthClient> logger, IMemoryCache cache, IConfiguration config)
    {
        _httpClient = httpClient;
        _logger = logger;
        _cache = cache;
        _config = config;
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
    }

    public async Task<string?> GetAccessToken()
    {
        var token = _cache.Get("ClientToken");
        if (token == null)
        {
            return await Authorize();
        }
        return token.ToString();
    }

    public async Task<string> Authorize()
    {
        HttpResponseMessage response;
        var email = _config["AuthLogin"];
        var password = _config["AuthPassword"];
        _logger.LogDebug($"Authorizing with email: {email}");
        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/users/sign-in")
        {
            Content = new StringContent(JsonSerializer.Serialize(new { email, password }), Encoding.UTF8, "application/json")
        };
        try
        {
            response = await _httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to authorize in auth service");
            throw new MicroserviceException("Failed to authorize in auth service");
        }
        var body = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<TokenData>(body, _jsonOptions)!.AccessToken;
        var info = TokenHelper.ParseToken(token);
        var lifetime = TokenHelper.GetTokenLifetime(info);
        _logger.LogInformation($"Got token with lifetime: {lifetime}");
        _cache.Set("ClientToken", token, lifetime);
        return token;
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