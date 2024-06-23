using System.Reflection;
using Microsoft.OpenApi.Models;
using svc_InterviewBack.Utils;
using Interns.Auth.Extensions;
using Interns.Common;
using Interns.Common.Middlewares;
using Interns.Common.SwaggerEnum;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// main configuration
services.AddServices(builder.Configuration);
services.AddCustomJsonOptions();

// swagger
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
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
        
        c.UseInlineDefinitionsForEnums();
    }
);

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

