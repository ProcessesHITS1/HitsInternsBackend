using Interns.Common;
using System.Text.Json.Serialization;

namespace svc_InterviewBack.Services.Clients;


public class ThirdCourseClient(HttpClient httpClient)
{

    public async Task AddStudentsToSemester(StudentsInternship studentsInSemester)
    {
        var response = await httpClient.PostAsJsonAsync("/api/students-in-semesters/transfer-to-third-course", studentsInSemester);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var error = await ClientHelper.DeserializeResponseAsync<ClientHelper.ErrorResponse>(response);
            throw new MicroserviceException(error.Messages.First());
        }
        response.EnsureSuccessStatusCode();
    }

    public record StudentsInternship
    {
        [JsonPropertyName("year")]
        public int Year { get; init; }

        [JsonPropertyName("seasonId")]
        public Guid SeasonId { get; init; }

        [JsonPropertyName("students")]
        public required List<StudentInfo> Students { get; init; }
    }

    public record StudentInfo
    {
        [JsonPropertyName("id")]
        public Guid StudentId { get; init; }

        [JsonPropertyName("companyId")]
        public Guid CompanyId { get; init; }
    }

}

