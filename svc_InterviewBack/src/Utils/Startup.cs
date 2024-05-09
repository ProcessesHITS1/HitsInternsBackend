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
        services.AddDbContext<InterviewDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        });
        return services;
    }


    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<InterviewDbContext>();
        context.Database.Migrate();
        return app;
    }
}
