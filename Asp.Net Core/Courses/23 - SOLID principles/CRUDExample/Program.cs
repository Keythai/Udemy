using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using OfficeOpenXml;
using Repositories;
using RepositoryContracts;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using CRUDExample.Filters.ActionFilters;
using CRUDExample;
using CRUDExample.Middleware;

var builder = WebApplication.CreateBuilder(args);
// Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration) // read configuration settings from built-in IConfiguration
        .ReadFrom.Services(services) // read out current app services and make them available to serilog
        .WriteTo.Console();
});

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

app.UseSerilogRequestLogging(); // Add Serilog request logging

app.UseHttpLogging();

// use Rotativa for generating PDF, only execute in non-test environment
if (builder.Environment.IsEnvironment("Test") == false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", "Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { } // make the auto-generated Program class accessible programmatically