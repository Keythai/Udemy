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
using ServiceContracts.FinnhubService;
using ServiceContracts.StockService;

namespace StockMarketSolution.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _options;
        private readonly IFinnhubPriceQuoteService _finnhubPriceQuoteService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IConfiguration _configuration;
        private readonly IBuyOrderService _buyOrderService;
        private readonly ISellOrderService _sellOrderService;
        private readonly ILogger<TradeController> _logger;
        public TradeController(
            IOptions<TradingOptions> options,
            IFinnhubPriceQuoteService finnhubPriceQuoteService,
            IFinnhubCompanyProfileService finnhubCompanyProfileService,
            IConfiguration configuration,
            IBuyOrderService buyOrderService,
            ISellOrderService sellOrderService,
            ILogger<TradeController> logger)
        {
            _options = options.Value;
            _finnhubPriceQuoteService = finnhubPriceQuoteService;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _configuration = configuration;
            _buyOrderService = buyOrderService;
            _sellOrderService = sellOrderService;
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
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubPriceQuoteService.GetStockPriceQuote(stockSymbol);
            Dictionary<string, object>? companyProfileDictionary = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);
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
            await _buyOrderService.CreateBuyOrder(orderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            await _sellOrderService.CreateSellOrder(orderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            Orders orders = new Orders()
            {
                BuyOrders = await _buyOrderService.GetBuyOrders(),
                SellOrders = await _sellOrderService.GetSellOrders()
            };
            return View(orders);
        }

        public async Task<IActionResult> OrdersPDF()
        {
            List<BuyOrderResponse> buyOrderResponse = await _buyOrderService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponse = await _sellOrderService.GetSellOrders();
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


