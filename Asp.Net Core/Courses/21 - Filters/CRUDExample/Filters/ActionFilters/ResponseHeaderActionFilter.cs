using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
    //demonstrate the flow: global > controller > method > method > controller > global
    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter //IOrderedFilter is a better way to set order
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string _key;
        private readonly string _value;
        public int Order { get; set; }
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value, int order)
        {
            _logger = logger;
            _key = key;
            _value = value;
            Order = order;
        }

        //after
        //public void OnActionExecuted(ActionExecutedContext context)
        //{
        //    _logger.LogInformation("{FilterName}.{MethodName} method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuted));

        //    context.HttpContext.Response.Headers[_key] = _value;
        //}

        //before
        //public void OnActionExecuting(ActionExecutingContext context)
        //{
        //    _logger.LogInformation("{FilterName}.{MethodName} method", nameof(ResponseHeaderActionFilter), nameof(OnActionExecuting));

        //    context.HttpContext.Response.Headers[_key] = _value;
            
        //}

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //before
            _logger.LogInformation("{FilterName}.{MethodName} method - before", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            context.HttpContext.Response.Headers[_key] = _value;

            await next();

            //after
            _logger.LogInformation("{FilterName}.{MethodName} method - after", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            context.HttpContext.Response.Headers[_key] = _value;

        }
    }
}
