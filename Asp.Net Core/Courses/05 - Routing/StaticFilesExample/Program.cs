using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = "myroot" // wwwroot is the default
});
var app = builder.Build();

app.UseStaticFiles(); // works with myroot
app.UseStaticFiles(new StaticFileOptions() // works with mywebroot
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath // C:\ASP.NET DB\Udemy\asp.net core\Routing\StaticFilesExample
        , "mywebroot")
 )});

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});

app.Run();
