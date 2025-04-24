using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repositories;
using ServiceContracts;
using StockMarketSolution.Models;

namespace StockMarketSolution.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IStocksService _stocksService;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;
        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IStocksService stocksService, IFinnhubService finnhubService, IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _stocksService = stocksService;
            _finnhubService = finnhubService;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
        {
            if (stockSymbol != null)
            {
                Dictionary<string, object>? companyProfileDict = await _finnhubService.GetCompanyProfile(stockSymbol);
                Dictionary<string, object>? stockPriceDict = await _finnhubService.GetStockPriceQuote(stockSymbol);
                if (companyProfileDict != null && stockPriceDict != null)
                {
                    companyProfileDict.Add("price", stockPriceDict["c"]);
                }

                // Check if the stock symbol is valid
                if (companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
                {
                    // Return the view with the stock symbol
                    return View("SelectedStock", companyProfileDict);
                }
            }
            // Handle the case where no data is found
            return Content("No data found for the given stock symbol.");
        }
    }
}
