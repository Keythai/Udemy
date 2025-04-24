using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        private string? Key {  get; set; }
        private string? Value { get; set; }
        private int Order { get; set; }
        public ResponseHeaderFilterFactoryAttribute(string key, string value, int order) 
        {
            Key = key;
            Value = value;
            Order = order;
        }

        //Controller -> FilterFactory -> Filter
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
            filter.Key = Key;
            filter.Value = Value;
            filter.Order = Order;
            //return filter object
            return filter;
        }
    }
    //demonstrate the flow: global > controller > method > method > controller > global
    //IOrderedFilter is a better way to set order
    //ActionFilterAttribute inherits IActionFilter, IFilterMetadata, IAsyncActionFilter, IResultFilter, IAsyncResultFilter, IOrderedFilter, simplifying the Attribute
    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            _logger = logger;
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
            _logger.LogInformation("Before logic - ResponseHeaderActionFilter");
            context.HttpContext.Response.Headers[Key] = Value;

            await next();

            //after
            _logger.LogInformation("After logic - ResponseHeaderActionFilter");
            context.HttpContext.Response.Headers[Key] = Value;

        }
    }
}
