using StockMarketSolution.Models;
using StockMarketSolution.Services;
using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

// use Rotativa for generating PDF
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
