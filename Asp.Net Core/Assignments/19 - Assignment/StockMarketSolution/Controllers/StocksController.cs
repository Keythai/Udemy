using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StockMarketSolution.Models;

namespace StockMarketSolution.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly TradingOptions _options;
        private readonly IFinnhubService _finnhubServices;
        public StocksController(IOptions<TradingOptions> options, IFinnhubService services)
        {
            _options = options.Value;
            _finnhubServices = services;
        }

        [Route("[action]/{stock?}")]
        [Route("~/[action]/{stock?}")]
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            if(_options.Top25PopularStocks == null)
                throw new InvalidOperationException("Cannot find popular stocks in configuration");
            string[] top25PopularStocks = _options.Top25PopularStocks.Split(',');
            
            List<Dictionary<string, string>>? stocksDictionary = await _finnhubServices.GetStocks();
            if(stocksDictionary == null)
                throw new InvalidOperationException("No response from finnhub server");
            if(!showAll)
                stocksDictionary = stocksDictionary
                    .Where(temp => top25PopularStocks.Contains(Convert.ToString(temp["symbol"])))
                    .ToList();

            List<Stocks> stocks = new List<Stocks>();
            stocks = stocksDictionary
                .Select(temp => new Stocks()
                {
                    StockSymbol = temp["symbol"],
                    StockName = temp["description"]
                })
                .ToList();
            ViewBag.Stock = stock;
            return View(stocks);
        }
    }
}
