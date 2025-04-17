using StockMarketSolution.Models;
using ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.DTO;
using Entities;
using Rotativa.AspNetCore;
using Microsoft.Extensions.Configuration;
using Services;

namespace StockMarketSolution.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _options;
        private readonly IFinnhubService _finnhubServices;
        private readonly IConfiguration _configuration;
        private readonly IStocksService _stocksService;
        private readonly ILogger<TradeController> _logger;
        public TradeController(IOptions<TradingOptions> options, IFinnhubService services, IConfiguration configuration, IStocksService stocksService, ILogger<TradeController> logger)
        {
            _options = options.Value;
            _finnhubServices = services;
            _configuration = configuration;
            _stocksService = stocksService;
            _logger = logger;
        }
        [Route("[action]/{stockSymbol}")]
        [HttpGet]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            // Log information
            _logger.LogInformation("In TradeController.Index() action method");
            _logger.LogDebug($"stockSymbol: {stockSymbol}");

            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubServices.GetStockPriceQuote(stockSymbol);
            Dictionary<string, object>? companyProfileDictionary = await _finnhubServices.GetCompanyProfile(stockSymbol);
            StockTrade stockTrade = new StockTrade();
            if (stockQuoteDictionary != null && companyProfileDictionary != null)
            {
                stockTrade = new StockTrade()
                {
                    Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()),
                    StockName = companyProfileDictionary["name"].ToString(),
                    StockSymbol = companyProfileDictionary["ticker"].ToString(),
                    Quantity = (uint)_options.DefaultOrderQuantity
                };
            }
            ViewBag.Token = _configuration["FinnhubToken"];
            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
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

            await _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
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

            await _stocksService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrderResponse = await _stocksService.GetBuyOrders();
            Orders orders = new Orders()
            {
                BuyOrders = await _stocksService.GetBuyOrders(),
                SellOrders = await _stocksService.GetSellOrders()
            };
            return View(orders);
        }

        public async Task<IActionResult> OrdersPDF()
        {
            List<BuyOrderResponse> buyOrderResponse = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponse = await _stocksService.GetSellOrders();
            List<IOrderResponse> orderResponses = new List<IOrderResponse>();
            orderResponses.AddRange(buyOrderResponse);
            orderResponses.AddRange(sellOrderResponse);

            return new ViewAsPdf("OrdersPDF", orderResponses, ViewData)
            {
                PageMargins = { Left = 20, Right = 20, Top = 20, Bottom = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}


