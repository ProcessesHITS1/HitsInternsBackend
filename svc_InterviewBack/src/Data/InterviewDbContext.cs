using Microsoft.EntityFrameworkCore;

namespace svc_InterviewBack.DAL;


public class InterviewDbContext(DbContextOptions<InterviewDbContext> options) : DbContext(options)
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<InterviewRequest> InterviewRequests { get; set; }
    public DbSet<RequestStatusSnapshot> RequestStatusSnapshots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Season>()
            .HasIndex(s => s.Year)
            .IsUnique();
        
        modelBuilder.Entity<InterviewRequest>()
            .HasOne(ir => ir.RequestStatusSnapshot)
            .WithOne(rs => rs.InterviewRequest)
            .HasForeignKey<RequestStatusSnapshot>(rs => rs.InterviewRequestId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }   //TODO: on position Creation -> update Season NPositions
}