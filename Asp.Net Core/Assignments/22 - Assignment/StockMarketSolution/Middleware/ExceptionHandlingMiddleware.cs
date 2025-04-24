using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace StockMarketSolution.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IDiagnosticContext diagnosticContext)
        {
            _next = next;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex) 
            {
                LogException(ex);
                throw;
            }
        }

        private void LogException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                if(ex.InnerException.InnerException != null)
                {
                    _logger.LogError("{ExceptionType}{ExceptionMessage}", ex.InnerException.InnerException.GetType().ToString(), ex.InnerException.InnerException.Message);
                    _diagnosticContext.Set("Exception", $"{ex.InnerException.InnerException.GetType().ToString()}, {ex.InnerException.InnerException.Message}, {ex.InnerException.InnerException.StackTrace}");
                }
                else
                {
                    _logger.LogError("{ExceptionType}{ExceptionMessage}", ex.InnerException.GetType().ToString(), ex.InnerException.Message);
                    _diagnosticContext.Set("Exception", $"{ex.InnerException.GetType().ToString()}, {ex.InnerException.Message}, {ex.InnerException.StackTrace}");
                }
            }
            else
            {
                _logger.LogError("{ExceptionType}{ExceptionMessage}", ex.GetType().ToString(), ex.Message);
                _diagnosticContext.Set("Exception", $"{ex.GetType().ToString()}, {ex.Message}, {ex.StackTrace}");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
