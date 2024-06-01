using Interns.Chats.Domain;
using Microsoft.EntityFrameworkCore;

namespace Interns.Chats.Persistence
{
    public class ChatDbContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }
    }
}
