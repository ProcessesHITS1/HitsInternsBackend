using Interns.Chats.Domain;
using Microsoft.EntityFrameworkCore;

namespace Interns.Chats.Persistence
{
    public class ChatDbContext : DbContext
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        DbSet<StoredFile> StoredFiles { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
