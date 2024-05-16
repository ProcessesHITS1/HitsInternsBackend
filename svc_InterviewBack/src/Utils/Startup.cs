using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Utils;

public static class Startup
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        // add cors
        services.AddCors();

        // add services
        services.TryAddScoped<ISeasonsService, SeasonsService>();
        services.AddAutoMapper(typeof(MapperProfile));

        // add db context
        services
        .AddDbContext<InterviewDbContext>(options =>
        {
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<InterviewDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            logger.LogInformation($"Using connection string: {connectionString}");
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

    private static IServiceCollection AddCors(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }
}
