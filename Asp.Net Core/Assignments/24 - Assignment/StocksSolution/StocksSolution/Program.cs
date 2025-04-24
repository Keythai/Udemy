using StockMarketSolution.Models;
using ServiceContracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using StockMarketSolution.Middleware;
using ServiceContracts.FinnhubService;
using Services.FinnhubService;
using Services.StockService;
using ServiceContracts.StockService;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
builder.Services.AddScoped<IFinnhubPriceQuoteService, FinnhubPriceQuoteService>();
builder.Services.AddScoped<IFinnhubSearchStocksService, FinnhubSearchStocksService>();
builder.Services.AddScoped<IFinnhubStocksService, FinnhubStocksService>();

builder.Services.AddScoped<IBuyOrderService, BuyOrderService>();
builder.Services.AddScoped<ISellOrderService, SellOrderService>();

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
