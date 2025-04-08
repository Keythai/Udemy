var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

// context.GetEndpoint() returns the endpoint that will handle the request, must be called after UseRouting
app.Use(async (context, next) =>
{
    Microsoft.AspNetCore.Http.Endpoint? endpoint = context.GetEndpoint();
    if (endpoint != null)
        await context.Response.WriteAsync($"Endpoint: {endpoint.DisplayName}");
    await next(context);
});

app.UseEndpoints(endpoints =>
{
    // Map applies to GET, POST, PUT, DELETE, etc.
    endpoints.Map("map1", async (context) =>
    {
        await context.Response.WriteAsync("In map 1");
    });
    endpoints.MapPost("map2", async (context) =>
    {
        await context.Response.WriteAsync("In map 2");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});
app.Run();
