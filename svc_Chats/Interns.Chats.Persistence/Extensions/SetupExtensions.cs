using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interns.Chats.Persistence.Extensions
{
    public static class SetupPersistance
    {
        public static WebApplicationBuilder AddPersistance(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ChatDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            return builder;
        }

        public static async Task UsePersistance(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var db = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
            await db.Database.MigrateAsync();
        }
    }
}
