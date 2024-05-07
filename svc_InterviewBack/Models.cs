namespace svc_InterviewBack.Models;

public record SeasonData(int Year, DateTime InterviewStart, DateTime InterviewEnd);
public record Season(Guid Id, int Year, DateTime InterviewStart, DateTime InterviewEnd);

public record SeasonDetails(Season Season, List<CompanyInSeasonInfo> SeasonData, List<StudentInfo> Students);

public record CompanyInSeasonInfo
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required int NPositions { get; init; }
}

public record StudentInfo
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string EmploymentStatus { get; init; }
}