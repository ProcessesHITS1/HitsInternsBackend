using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Services;

namespace svc_InterviewBack.Utils;

public static class Utils
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        // add services
        services.TryAddScoped<ISeasonsService, SeasonsService>();

        // add db context
        services.AddDbContext<InterviewDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        });
        return services;
    }
}
