using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Services.Clients;


public class ThirdCourseClient(HttpClient httpClient, AuthClient authClient, IMemoryCache cache) : BaseClient(httpClient, authClient, cache)
{

    public async Task<Semesters> GetSemesters(int pageNumber, int pageSize)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/third-course/semesters?page={pageNumber}&size={pageSize}");

        var response = await SendWithAuth(request);
        return await DeserializeResponse<Semesters>(response);
    }

    public async Task AddStudentsToSemester(StudentsInSemester studentsInSemester)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/third-course/students-in-semester")
        {
            Content = new StringContent(JsonSerializer.Serialize(studentsInSemester), Encoding.UTF8, "application/json")
        };

        var response = await SendWithAuth(request);
        response.EnsureSuccessStatusCode();
    }

    public record StudentsInSemester
    {
        public required List<StudentInSemester> StudentInSemester { get; init; }
    }

    public record StudentInSemester
    {
        public Guid StudentId { get; init; }
        public Guid CompanyId { get; init; }
        public Guid SemesterId { get; init; }
        public Guid DiaryId { get; init; }
        public bool InternshipPassed { get; init; }
    }

    public record PageInfo
    {
        public int Total { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }

    public record SemesterInfo
    {
        public Guid Id { get; init; }
        public int Year { get; init; }
        public int Semester { get; init; }
        public Guid SeasonId { get; init; }
        public DateTime DocumentsDeadline { get; init; }
    }

    public record Semesters
    {
        public required PageInfo PageInfo { get; init; }
        public required List<SemesterInfo> Data { get; init; }
    }
}

