namespace svc_InterviewBack.DAL;


// Basically these entities represent the tables in the database
public record Company
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required Season Season { get; init; }
    public required List<Position> Positions { get; init; }
}

public record Student
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required Season Season { get; init; }
    public required EmploymentStatus EmploymentStatus { get; init; }
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
    public DateTime SeasonStart { get; init; }
    public DateTime SeasonEnd { get; init; }
    public required List<Company> Companies { get; init; }
    public required List<Student> Students { get; init; }
};

// Job position in a company
public record Position
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    // Number of positions available
    public required int NPositions { get; set; }
};


// Interview request from a student to a position in a company
public record InterviewRequest
{
    public Guid Id { get; init; }
    public required Student Student { get; init; }
    public required Position Position { get; init; }
    public ResultStatus Status { get; init; } = ResultStatus.Pending;
};


public enum ResultStatus
{
    Pending,
    Accepted,
    Rejected
}

public enum RequestStatus
{
    Waiting,
    TestGiven,
    Interviewed,
    Done,
    Canceled
}
