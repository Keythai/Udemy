
using Microsoft.Extensions.Primitives;

namespace assignment
{
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string adminEmail = "admin@example.com", adminPassword = "admin1234";

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(context.Request.Method == "POST" && context.Request.Path == "/")
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(context.Request.Body);
                string body = await reader.ReadToEndAsync();
                Dictionary<string, StringValues> queryDict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);
                string? email = null, password = null;
                if (queryDict.ContainsKey("email"))
                {
                    email = queryDict["email"];
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Email is required");
                    return;
                }
                if(queryDict.ContainsKey("password"))
                {
                    password = queryDict["password"];
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Password is required");
                    return;
                }
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    if(email == adminEmail && password == adminPassword)
                    {
                        await context.Response.WriteAsync("Successful login");
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Invalid login");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Email and password cannot be blank");
                    return;
                }
            }
            await _next(context);
        }
    }
    public static class LoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoginMiddleware>();
        }
    }
}
