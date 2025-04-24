using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts.DTO;
using StockMarketSolution.Controllers;
using StockMarketSolution.Models;

namespace StockMarketSolution.Filters.ActionFilters
{
    /// <summary>
    /// An action filter that applies model validation to both SellOrder() and BuyOrder() action methods in TradeController
    /// </summary>
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is TradeController tradeController)
            {
                var orderRequest = context.ActionArguments["orderRequest"] as IOrderRequest;
                if (orderRequest != null)
                {
                    orderRequest.DateAndTimeOfOrder = DateTime.Now;
                    tradeController.ModelState.Clear();
                    tradeController.TryValidateModel(orderRequest);
                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                        StockTrade stockTrade = new StockTrade() { StockName = orderRequest.StockName, Price = orderRequest.Price, Quantity = orderRequest.Quantity, StockSymbol = orderRequest.StockSymbol };
                        context.Result = tradeController.View("Index", stockTrade);
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}
