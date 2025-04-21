using StockMarketSolution.Models;
using ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.DTO;
using Entities;
using Rotativa.AspNetCore;
using Microsoft.Extensions.Configuration;
using Services;
using StockMarketSolution.Filters.ActionFilters;

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
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            await _stocksService.CreateBuyOrder(orderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            await _stocksService.CreateSellOrder(orderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
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


