using svc_InterviewBack.Middlewares;
using svc_InterviewBack.Utils;

var builder = WebApplication.CreateBuilder(args);

// main configuration
builder.Services.AddServices(builder.Configuration);
builder.Services.AddControllers();

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var isDev = builder.Environment.IsDevelopment();

// Configure the HTTP request pipeline.
if (isDev)
{
    app.UseSwagger()
    .UseSwaggerUI();
}
app.UseHttpsRedirection();

// Enable debug in dev environment
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api/debug") && !isDev)
    {
        context.Response.StatusCode = 404;
        return;
    }
    await next.Invoke();
});

app.UseErrorHandlingMiddleware();
app.MapControllers();
app.Run();

