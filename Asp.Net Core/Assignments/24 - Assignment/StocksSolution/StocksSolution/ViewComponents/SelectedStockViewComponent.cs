using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Repositories;
using ServiceContracts;
using ServiceContracts.FinnhubService;
using ServiceContracts.StockService;
using StockMarketSolution.Models;

namespace StockMarketSolution.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubPriceQuoteService _finnhubPriceQuoteService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IConfiguration _configuration;
        public SelectedStockViewComponent(
            IOptions<TradingOptions> tradingOptions,
            IFinnhubPriceQuoteService finnhubPriceQuoteService,
            IFinnhubCompanyProfileService finnhubCompanyProfileService,
            IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubPriceQuoteService = finnhubPriceQuoteService;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
        {
            if (stockSymbol != null)
            {
                Dictionary<string, object>? companyProfileDict = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);
                Dictionary<string, object>? stockPriceDict = await _finnhubPriceQuoteService.GetStockPriceQuote(stockSymbol);
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
