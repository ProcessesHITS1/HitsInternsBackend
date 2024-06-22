using System.ComponentModel.DataAnnotations;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Utils;

namespace svc_InterviewBack.Models;

// Basic season models
// ################

// Creation model
public record SeasonData
{
    [SeasonYearRange]
    public int Year { get; init; }
    public DateTime SeasonStart { get; init; }
    public DateTime SeasonEnd { get; init; }
}
// Returned to the client
public record Season : SeasonData
{
    public Guid Id { get; init; }
    public bool IsClosed { get; init; }
}

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
    public required int NPositions { get; set; }
}


// Returned to the client
public record StudentInfo
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string EmploymentStatus { get; init; }
    public Guid? CompanyId { get; init; }
}

// On creation
public record PositionCreation
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    [NSeatsRange]
    public int NSeats { get; init; }
    public Guid CompanyId { get; set; }
    [SeasonYearRange]
    public int SeasonYear { get; set; }

}

public record PositionUpdate
{
    public string? Title { get; init; }
    public string? Description { get; init; }
    [NSeatsRange]
    public int?  NSeats { get; init; }
}
// On search
public record PositionQuery
{
    public string Query { get; init; } = "";
    public List<Guid> CompanyIds { get; init; } = [];
    [SeasonYearRange]
    public int SeasonYear { get; init; }
}


// Returned to the client
public record PositionInfo : PositionCreation
{
    public Guid Id { get; init; }
    public int NRequests { get; init; }
    public required string CompanyName { get; init; }
}

public record RequestQuery
{
    public List<Guid> StudentIds { get; init; } = [];
    public List<Guid> CompanyIds { get; init; } = [];
    public List<int> SeasonYears { get; init; } = [];
    public bool IncludeHistory { get; init; }
}

public record RequestData
{
    public Guid Id { get; init; }
    public Guid StudentId { get; init; }
    public Guid CompanyId { get; init; }
    public string StudentName { get; init; }
    public Guid PositionId { get; init; }
    public string PositionTitle { get; init; }
    public List<RequestStatusSnapshotData> RequestStatusSnapshots { get; init; } = [];
    public RequestResultData? RequestResult { get; init; }
}
public record RequestDetails
{
    public Guid Id { get; init; }
    public required Guid StudentId { get; init; }
    public required Guid PositionId { get; init; }
    public ResultStatus Status { get; init; }
}

public record RequestResultData
{
    public ResultStatus ResultStatus { get; init; }
    public string? Description { get; init; }
    public bool OfferGiven { get; init; }
}

public record RequestResultUpdate
{
    public ResultStatus? ResultStatus { get; init; }
    public string? Description { get; init; }
    public bool? OfferGiven { get; init; }
}
public record RequestStatusSnapshotData
{
    public Guid Id { get; init; }
    public DateTime DateTime { get; init; }
    public string Status { get; init; }
}

public record RequestStatusTemplateData
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}
