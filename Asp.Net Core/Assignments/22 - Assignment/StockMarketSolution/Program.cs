using StockMarketSolution.Models;
using StockMarketSolution.Services;
using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using StockMarketSolution.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddScoped<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddScoped<IStocksRepository, StocksRepository>();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .WriteTo.Console();
});

var app = builder.Build();

// use Rotativa for generating PDF
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseExceptionHandlingMiddleware();
}

if (builder.Environment.IsEnvironment("Test")==false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.UseSerilogRequestLogging();

app.Run();

public partial class Program { }
