using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

Dictionary<int, string> countries = new Dictionary<int, string>
{
    {1, "United States"},
    {2, "Canada"},
    {3, "United Kingdom"},
    {4, "India" },
    {5, "Japan" }
};

app.UseEndpoints(endpoints =>
{
    app.MapGet("countries", async context =>
    {
        foreach (var country in countries)
        {
            await context.Response.WriteAsync($"{country.Key}, {country.Value}\n");
        }
    });

    app.MapGet("countries/{countryID:int:range(1,100)}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("countryID"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["countryID"]);
            foreach (var country in countries)
            {
                if (country.Key == id)
                {
                    await context.Response.WriteAsync($"{country.Value}\n");
                    return;
                }
            }
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("No country\n");
            return;
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("No country\n");
            return;
        }
    });

    app.MapGet("countries/{countryID:int:min(101)}", async context =>
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("The CountryID should be between 1 and 100\n");
    });
});
app.Run();
