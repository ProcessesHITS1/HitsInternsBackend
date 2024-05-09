namespace svc_InterviewBack.DAL;


// Basically these entities represent the tables in the database
public record Company(Guid Id, string Name);

public record Student(Guid Id, string Name);

public record Season
{
    public Guid Id { get; init; }
    public int Year { get; init; }
    public DateTime InterviewStart { get; init; }
    public DateTime InterviewEnd { get; init; }
    public required List<Company> Companies { get; init; }
    public required List<Student> Students { get; init; }
};

// Job position in a company
public record Position
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    // Number of positions available
    public required int N { get; init; }
    public required Company Company { get; init; }
};


// Interview request from a student to a position in a company
public record InterviewRequest
{
    public Guid Id { get; init; }
    public required Student Student { get; init; }
    public required Position Position { get; init; }
    public RequestStatus Status { get; init; }
};


public enum RequestStatus
{
    Pending,
    Accepted,
    Rejected
}