using System.Reflection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using svc_InterviewBack.Middlewares;
using svc_InterviewBack.Utils;
using Interns.Auth.Extensions;

var builder = WebApplication.CreateBuilder(args);
// main configuration
builder.Services.AddServices(builder.Configuration);
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });


// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        // docs
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Interview service api", Version = "v1" });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
        // auth
        c.AddAuth();
        
        c.UseAllOfToExtendReferenceSchemas();
        c.UseOneOfForPolymorphism();
    }
);
builder.Services.AddSwaggerGenNewtonsoftSupport();

// auth
builder.ConfigureAuth();
var app = builder.Build();
var isDev = builder.Environment.IsDevelopment();

// Configure the HTTP request pipeline.
if (isDev)
{
    app.UseSwagger()
    .UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors();
app.UseErrorHandlingMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

