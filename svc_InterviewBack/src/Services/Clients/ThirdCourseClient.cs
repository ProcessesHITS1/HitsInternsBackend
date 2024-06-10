using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services.Clients;


public class ThirdCourseClient(HttpClient httpClient)
{

    public async Task AddStudentsToSemester(StudentsInternship studentsInSemester)
    {
        var request = ClientHelper.SerializeRequest(HttpMethod.Post, "/api/students-in-semesters/transfer-to-third-course", studentsInSemester);
        var response = await httpClient.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var error = await ClientHelper.DeserializeResponseAsync<ClientHelper.ErrorResponse>(response);
            throw new MicroserviceException(error.Messages.First());
        }
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

