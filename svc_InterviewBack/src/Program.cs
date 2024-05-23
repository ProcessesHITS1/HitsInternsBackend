using System.Reflection;
using Microsoft.OpenApi.Models;
using svc_InterviewBack.Middlewares;
using svc_InterviewBack.Utils;

var builder = WebApplication.CreateBuilder(args);
// main configuration
builder.Services.AddServices(builder.Configuration);
builder.Services.AddControllers();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Interview service api", Version = "v1" });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    }
);

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
app.MapControllers();
app.Run();

