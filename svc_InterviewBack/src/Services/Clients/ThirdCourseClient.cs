using System.Text;
using System.Text.Json;

namespace svc_InterviewBack.Services.Clients;


public class ThirdCourseClient(HttpClient httpClient)
{

    public async Task AddStudentsToSemester(StudentsInternship studentsInSemester)
    {
        var request = ClientHelper.SerializeRequest(HttpMethod.Post, "/api/students-in-semesters/transfer-to-third-course", studentsInSemester);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public record StudentsInternship
    {
        public int Year { get; init; }
        public required List<StudentInfo> Students { get; init; }
    }

    public record StudentInfo
    {
        public Guid StudentId { get; init; }
        public Guid CompanyId { get; init; }
    }

}

