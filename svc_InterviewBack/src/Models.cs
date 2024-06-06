using System.ComponentModel.DataAnnotations;
using svc_InterviewBack.DAL;

namespace svc_InterviewBack.Models;

// Basic season models
// ################

// Creation model
public record SeasonData(
    [Range(2010, 3000, ErrorMessage = "Year must be between 2010 and 3000")]
    int Year,
    DateTime SeasonStart,
    DateTime SeasonEnd);

// Returned to the client
public record Season(Guid Id, int Year, DateTime SeasonStart, DateTime SeasonEnd);

// Returned to the client
public record SeasonDetails
{
    public required Season Season { get; init; }
    public required List<CompanyInSeasonInfo> Companies { get; init; }
    public required List<StudentInfo> Students { get; init; }
};

// Company related models

// Returned to the client
public record CompanyInSeasonInfo
{
    public Guid Id { get; init; }
    public int SeasonYear { get; init; }
    public required string Name { get; init; }
    public required int NPositions { get; init; }
}


// Returned to the client
public record StudentInfo
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string EmploymentStatus { get; init; }
}

// On creation
public record PositionData
{
    public required string? Title { get; init; }
    public string? Description { get; init; }
    [Range(1, int.MaxValue, ErrorMessage = "NPosition must be 1 or more")]
    public int? NPositions { get; init; }

}

// On search
public record PositionQuery
{
    public string Query { get; init; } = "";
    public List<Guid> CompanyIds { get; init; } = [];
}


// Returned to the client
public record PositionInfo : PositionData
{
    public Guid Id { get; init; }
}

public record PositionDetails
{
    public required PositionInfo PositionInfo { get; init; }
    public CompanyInSeasonInfo? CompanyInfo { get; init; }
}

public record RequestDetails
{
    public Guid Id { get; init; }
    public required Guid StudentId { get; init; }
    public required Guid PositionId { get; init; }
    public ResultStatus Status { get; init; }
}