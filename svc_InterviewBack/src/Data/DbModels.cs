using Microsoft.EntityFrameworkCore;


namespace svc_InterviewBack.DAL;

// Basically these entities represent the tables in the database
[PrimaryKey(nameof(Id), nameof(SeasonId))]
public record Company
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required Guid SeasonId { get; init; }
    public required Season Season { get; init; }
    public required List<Position> Positions { get; init; }
}

[PrimaryKey(nameof(Id), nameof(SeasonId))]
public record Student
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required Guid SeasonId { get; init; }
    public required Season Season { get; init; }
    public required EmploymentStatus EmploymentStatus { get; set; }
    public Company? Company { get; set; }
    public required List<InterviewRequest> InterviewRequests { get; init; }
}

// This status must be updated every time the student's interview requests are changed
public enum EmploymentStatus
{
    Employed,
    Unemployed
}

public record Season
{
    public Guid Id { get; init; }
    public int Year { get; init; }
    public bool IsClosed { get; set; }
    public DateTime SeasonStart { get; set; }
    public DateTime SeasonEnd { get; set; }
    public required List<Company> Companies { get; init; }
    public required List<Student> Students { get; init; }

    public List<RequestStatusTemplate> RequestStatusTemplates { get; set; } = [];
};

// Job position in a company
public record Position
{
    public Guid Id { get; init; }
    public Company Company { get; init; }
    public required string Title { get; set; }

    public string? Description { get; set; }

    // Number of positions available
    public required int NSeats { get; set; }
};

public record CompanyAndPosition(Company Company, Position Position);

// Interview request from a student to a position in a company
public record InterviewRequest
{
    public Guid Id { get; init; }
    public required Student Student { get; init; }
    public required Position Position { get; init; }
    public RequestResult? RequestResult { get; set; }

    public ICollection<RequestStatusSnapshot> RequestStatusSnapshots { get; set; } =
        new List<RequestStatusSnapshot>(); // History of status snapshots
};

public record RequestResult
{
    public Guid Id { get; init; }
    public string? Description { get; set; } //TODO:null or ""
    public bool OfferGiven { get; set; }
    public ResultStatus StudentResultStatus { get; set; }
    public ResultStatus SchoolResultStatus { get; set; }

    public bool IsEmployed()
    {
        return
            OfferGiven &&
            StudentResultStatus == ResultStatus.Accepted &&
            SchoolResultStatus == ResultStatus.Accepted;
    }
}

public enum ResultStatus
{
    Pending,
    Accepted,
    Rejected
}

public record RequestStatusSnapshot
{
    public Guid Id { get; init; }
    public DateTime DateTime { get; init; }
    //TODO: add description 

    public RequestStatusTemplate RequestStatusTemplate { get; init; }
    public InterviewRequest InterviewRequest { get; init; }
}

public record RequestStatusTemplate
{
    public Guid Id { get; init; }
    public string Name { get; init; }

    public List<Season> Seasons { get; set; } = [];
}