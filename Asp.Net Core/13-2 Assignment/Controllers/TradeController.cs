using _13_2_Assignment.Models;
using _13_2_Assignment.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace _13_2_Assignment.Controllers
{
    public class TradeController : Controller
    {
        private readonly TradingOptions _options;
        private readonly IFinnhubService _finnhubServices;
        private readonly IConfiguration _configuration;
        public TradeController(IOptions<TradingOptions> options, IFinnhubService services, IConfiguration configuration)
        {
            _options = options.Value;
            _finnhubServices = services;
            _configuration = configuration;
        }
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(_options.DefaultStockSymbol))
                _options.DefaultStockSymbol = "MSFT";
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubServices.GetStockPriceQuote(_options.DefaultStockSymbol);
            Dictionary<string, object>? companyProfileDictionary = await _finnhubServices.GetCompanyProfile(_options.DefaultStockSymbol);
            StockTrade stockTrade = new StockTrade();
            if (stockQuoteDictionary != null && companyProfileDictionary != null)
            {
                stockTrade = new StockTrade()
                {
                    Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()),
                    StockName = companyProfileDictionary["name"].ToString(),
                    StockSymbol = companyProfileDictionary["ticker"].ToString()
                };
            }
            ViewBag.Token = _configuration["FinnhubToken"];
            return View(stockTrade);
        }
    }
}
