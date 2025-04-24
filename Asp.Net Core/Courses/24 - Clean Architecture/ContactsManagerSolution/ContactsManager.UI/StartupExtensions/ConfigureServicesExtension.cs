using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;

namespace CRUDExample
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ResponseHeaderActionFilter>();
            services.AddControllersWithViews(options =>
            {
                //Order in 5, but not supporting parameters
                //options.Filters.Add<ResponseHeaderActionFilter>(5);

                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
                //this support passing parameters
                options.Filters.Add(new ResponseHeaderActionFilter(logger) { Key = "My-_key-From-Global", Value = "My-_value-From-Global", Order = 2 });
            });

            // Add services, scoped because persondbcontext is scoped
            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();

            services.AddScoped<ICountriesAdderService, CountriesAdderService>();
            services.AddScoped<ICountriesGetterService, CountriesGetterService>();
            services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();

            //Open/Closed Principle - modify service by adding and applying new service, without modifying the original service
            services.AddScoped<IPersonsGetterService, PersonsGetterServiceWithFewExcelFields>();
            services.AddScoped<PersonsGetterService, PersonsGetterService>();

            services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
            services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
            services.AddScoped<IPersonsAdderService, PersonsAdderService>();
            services.AddScoped<IPersonsSorterService, PersonsSorterService>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestProperties |
                HttpLoggingFields.ResponsePropertiesAndHeaders;
            });
            return services;
        }
    }
}
