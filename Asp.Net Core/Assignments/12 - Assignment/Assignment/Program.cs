using ServiceContract;
using Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IWeatherService, WeatherService>();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
