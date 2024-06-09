using System.Text;
using System.Text.Json;

namespace svc_InterviewBack.Services.Clients;


public class ThirdCourseClient(HttpClient httpClient)
{

    public async Task AddStudentsToSemester(StudentsInternship studentsInSemester)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/students-in-semester/transfer-to-third-course")
        {
            Content = new StringContent(JsonSerializer.Serialize(studentsInSemester), Encoding.UTF8, "application/json")
        };

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public record StudentsInternship
    {
        public int Year { get; init; }
        public required List<StudentInfo> StudentInSemester { get; init; }
    }

    public record StudentInfo
    {
        public Guid StudentId { get; init; }
        public Guid CompanyId { get; init; }
    }

}

