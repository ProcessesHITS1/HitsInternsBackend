namespace svc_InterviewBack.Models;

public record SeasonData(int Year, DateTime SeasonStart, DateTime SeasonEnd);
public record Season(int Year, DateTime SeasonStart, DateTime SeasonEnd);

public record SeasonDetails
{
    public required Season Season { get; init; }
    public required List<CompanyInSeasonInfo> Companies { get; init; }
    public required List<StudentInfo> Students { get; init; }
};

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