using StockMarketSolution.Models;
using ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.DTO;
using Entities;

namespace StockMarketSolution.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _options;
        private readonly IFinnhubService _finnhubServices;
        private readonly IConfiguration _configuration;
        private readonly IStocksService _stocksService;
        public TradeController(IOptions<TradingOptions> options, IFinnhubService services, IConfiguration configuration, IStocksService stocksService)
        {
            _options = options.Value;
            _finnhubServices = services;
            _configuration = configuration;
            _stocksService = stocksService;
        }
        [Route("/")]
        [Route("[action]")]
        [HttpGet]
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

        [Route("[action]")]
        [HttpPost]
        public IActionResult BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            ModelState.Clear();
            bool isValid = TryValidateModel(buyOrderRequest);
            if (!isValid) { 
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View("Index", new StockTrade()
                {
                    StockName = buyOrderRequest.StockName,
                    StockSymbol = buyOrderRequest.StockSymbol,
                    Price = buyOrderRequest.Price,
                    Quantity = buyOrderRequest.Quantity
                }); 
            }

            _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult SellOrder(SellOrderRequest sellOrderRequest)
        {
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            ModelState.Clear();
            bool isValid = TryValidateModel(sellOrderRequest);
            if (!isValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View("Index", new StockTrade()
                {
                    StockName = sellOrderRequest.StockName,
                    StockSymbol = sellOrderRequest.StockSymbol,
                    Price = sellOrderRequest.Price,
                    Quantity = sellOrderRequest.Quantity
                });
            }

            _stocksService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Orders()
        {
            List<BuyOrderResponse> buyOrderResponse = _stocksService.GetBuyOrders();
            Orders orders = new Orders()
            {
                BuyOrders = _stocksService.GetBuyOrders(),
                SellOrders = _stocksService.GetSellOrders()
            };
            return View(orders);
        }

        public IActionResult OrdersPDF()
        {
            return View();
        }
    }
}


