using Microsoft.EntityFrameworkCore;

namespace svc_InterviewBack.DAL;


public class InterviewDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<InterviewRequest> InterviewRequests { get; set; }

    public InterviewDbContext(DbContextOptions<InterviewDbContext> options) : base(options)
    {
    }
}