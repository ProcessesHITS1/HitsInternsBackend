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
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// Enable debug in dev environment
app.Use(async (context, next) =>
{
    if (isDev && context.Request.Path.StartsWithSegments("/api/debug"))
    {
        await next.Invoke();
    }
    else
    {
        context.Response.StatusCode = 404;
        return;
    }
});

app.MapControllers();
app.Run();

