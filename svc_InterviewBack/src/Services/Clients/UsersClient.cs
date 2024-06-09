using svc_InterviewBack.Utils;
using System.Text.Json.Serialization;

namespace svc_InterviewBack.Services.Clients;


public class UsersClient(HttpClient httpClient)
{

    public async Task<User> GetUser(Guid id)
    {
        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"/api/users/{id}"));
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new NotFoundException($"User with id {id} not found");
        }
        return await ClientHelper.DeserializeResponse<User>(response);
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