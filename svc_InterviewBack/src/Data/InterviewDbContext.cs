using Microsoft.EntityFrameworkCore;

namespace svc_InterviewBack.DAL;


public class InterviewDbContext(DbContextOptions<InterviewDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<InterviewRequest> InterviewRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Season>()
            .HasIndex(s => s.Year)
            .IsUnique();
    }   //TODO: on position Creation -> update Season NPositions
}