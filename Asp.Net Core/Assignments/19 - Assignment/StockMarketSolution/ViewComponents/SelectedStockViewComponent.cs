using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace StockMarketSolution.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubRepository _finnhubRepository;
        public SelectedStockViewComponent(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
        {
            Dictionary<string, object>? companyProfileDict = await _finnhubRepository.SearchStocks(stockSymbol);
            Dictionary<string, object>? stockPriceDict = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
            if (companyProfileDict != null && stockPriceDict != null)
            {
                companyProfileDict.Add("price", stockPriceDict["c"]);
            }

            // Check if the stock symbol is valid
            if(companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
            {
                // Return the view with the stock symbol
                return View("SelectedStock", companyProfileDict);
            }
            // Handle the case where no data is found
            return Content("No data found for the given stock symbol.");
        }
    }
}
