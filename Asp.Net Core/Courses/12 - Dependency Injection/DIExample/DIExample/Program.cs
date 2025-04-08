using Autofac;
using Autofac.Extensions.DependencyInjection;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllersWithViews();

// for dependency injection
//builder.Services.Add(new ServiceDescriptor(
//    typeof(ICitiesService),  // first argument: interface
//    typeof(CitiesService),  // second argument: class
//    ServiceLifetime.Scoped // object created > transient: per injection, scoped: per browser request, singleton: per entire application lifetime
//));

// Autofac
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerLifetimeScope();
});

// short hand version
//builder.Services.AddScoped<ICitiesService, CitiesService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
