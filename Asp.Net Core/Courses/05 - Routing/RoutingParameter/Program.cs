using RoutingParameters.CustomConstraint;

var builder = WebApplication.CreateBuilder(args);
// register the custom constraint as 'months'
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map("files/{filename}.{extension}", async context =>
    {
        string? filename = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
        await context.Response.WriteAsync($"In files - {filename} - {extension}");
    });

    // defining default value to the route parameter
    endpoints.Map("employee/profile/{employeename=harsha}", async context =>
    {
        string? employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
        await context.Response.WriteAsync($"Employee name: {employeeName}");
    });

    // defining optional route parameter with data type constraint
    endpoints.Map("products/details/{id:int?}", async context =>
    {
        int? id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product id: {id}");
    });

    // another data type constraint example
    endpoints.Map("daily-digest-report/{reportdate:datetime}", async context =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"Daily digest report: {reportDate.ToShortDateString()}");
    });

    // guid parameter
    endpoints.Map("cities/{cityid:guid}", async context =>
    {
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
        await context.Response.WriteAsync($"City Id: {cityId}");
    });

    // length constraint, same with minlength(4):maxlength(7)
    // value constraint: min(1):max(10) = range(1,10) for integer values
    // alpha constraint: only alphabet is allowed
    // regex constraint: regex(^[0-9]{3}$) = only 3 digit number is allowed
    endpoints.Map("length/sample/{samplename:length(4,7)=harsha}", async context =>
    {
        string? sampleName = Convert.ToString(context.Request.RouteValues["samplename"]);
        await context.Response.WriteAsync($"Employee name: {sampleName}");
    });

    // regex constraint example in custom constraint
    // **Note: all constraints are not recommended to use when validating data, because it doesn't provide proper error message
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string month = Convert.ToString(context.Request.RouteValues["month"]);
        await context.Response.WriteAsync($"Sales report: {year} - {month}");
    });

    // endpoint selection order
    // this one has higher precedence than the above one because it's more specific
    endpoints.Map("sales-report/2024/jan", async context =>
    {
        await context.Response.WriteAsync("Sales report exclusively for 2024 Jan");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at: {context.Request.Path}");
});
app.Run();
