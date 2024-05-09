using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Utils;

public static class Startup
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        // add services
        services.TryAddScoped<ISeasonsService, SeasonsService>();
        services.AddAutoMapper(typeof(MapperProfile));

        // add db context
        services
        .AddDbContext<InterviewDbContext>(options =>
        {
            var connectionString = config.GetConnectionString("DefaultConnection") ?? Environment.GetEnvironmentVariable("CONNECTION_STRING");
            options.UseNpgsql(connectionString);
        })
        .MigrateDatabase(config);
        return services;
    }


    private static IServiceCollection MigrateDatabase(this IServiceCollection services, IConfiguration config)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<InterviewDbContext>();
        context.Database.Migrate();
        return services;
    }
}
