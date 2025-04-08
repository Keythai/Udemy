using MiddlewareExample.Middleware;

var builder = WebApplication.CreateBuilder(args);
// Add custom middleware in other folder
builder.Services.AddTransient<CustomMiddleware>();
var app = builder.Build();

app.Use(async (HttpContext context, RequestDelegate _next) =>
{
    await context.Response.WriteAsync("From Middleware 1\n");
    await _next(context);
});

// Use custom middleware
//app.UseMiddleware<CustomMiddleware>();
// A more efficient way
//app.UseCustomMiddleware();
app.UseHelloCustomMiddleware();

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("From Middleware 3\n");
});

app.Run();
