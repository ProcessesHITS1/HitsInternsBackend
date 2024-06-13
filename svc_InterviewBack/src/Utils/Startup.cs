using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using svc_InterviewBack.DAL;
using svc_InterviewBack.Services;
using svc_InterviewBack.Services.Clients;

namespace svc_InterviewBack.Utils;

public static class Startup
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        // add other dependencies
        services
        .AddCors()
        .AddMemoryCache()
        .AddAutoMapper(typeof(MapperProfile));

        // add services
        services
        .AddScoped<ISeasonsService, SeasonsService>()
        .AddScoped<IPositionService, PositionsService>()
        .AddScoped<ICompaniesService, CompaniesService>()
        .AddScoped<IStudentsService, StudentsService>()
        .AddScoped<IRequestService, RequestService>();

        // add clients
        services.AddTransient<AuthHandler>();
        ConfigureHttpClient<CompaniesClient>(services, config["CompaniesServiceUrl"]!);
        ConfigureHttpClient<UsersClient>(services, config["AuthServiceUrl"]!);
        ConfigureHttpClient<ThirdCourseClient>(services, config["ThirdCourseServiceUrl"]!);
        services.AddHttpClient<AuthClient>(client =>
        {
            client.BaseAddress = new Uri(config["AuthServiceUrl"]!);
        });

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

    private static void ConfigureHttpClient<TClient>(IServiceCollection services, string serviceUrl) where TClient : class
    {
        services.AddHttpClient<TClient>(client =>
        {
            client.BaseAddress = new Uri(serviceUrl);
        }).AddHttpMessageHandler<AuthHandler>();
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
