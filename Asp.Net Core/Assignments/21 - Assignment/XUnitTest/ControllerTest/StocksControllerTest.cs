﻿using AutoFixture;
using Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Repositories;
using ServiceContracts;
using StockMarketSolution.Controllers;
using StockMarketSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTest.ControllerTest
{
    public class StocksControllerTest
    {
        private readonly IFinnhubService _finnhubService;
        private readonly Mock<IFinnhubService> _finnhubServiceMock;
        private readonly IFixture _fixture;
        public StocksControllerTest()
        {
            _fixture = new Fixture();
            _finnhubServiceMock = new Mock<IFinnhubService>();
            _finnhubService = _finnhubServiceMock.Object;
        }

        #region Index

        //1. When you supply null to "stock" parameter, it should return "Explore" view along with List<Stock> as model object.
        [Fact]
        public async Task Explore_NullStock_ReturnsExploreView()
        {
            // Arrange
            IOptions<TradingOptions> _options = Options.Create(new TradingOptions()
            {
                DefaultOrderQuantity = 100,
                Top25PopularStocks = "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO"
            });
            StocksController stocksController = new StocksController(_options, _finnhubService);

            // Act
            List<Dictionary<string, string>>? stocksDict = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(@"[{'currency':'USD','description':'APPLE INC','displaySymbol':'AAPL','figi':'BBG000B9XRY4','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5N8V8','symbol':'AAPL','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'MICROSOFT CORP','displaySymbol':'MSFT','figi':'BBG000BPH459','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5TD05','symbol':'MSFT','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'AMAZON.COM INC','displaySymbol':'AMZN','figi':'BBG000BVPV84','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5PQL7','symbol':'AMZN','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'TESLA INC','displaySymbol':'TSLA','figi':'BBG000N9MNX3','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001SQKGD7','symbol':'TSLA','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'ALPHABET INC-CL A','displaySymbol':'GOOGL','figi':'BBG009S39JX6','isin':null,'mic':'XNAS','shareClassFIGI':'BBG009S39JY5','symbol':'GOOGL','symbol2':'','type':'Common Stock'}]");
            _finnhubServiceMock.Setup(temp => temp.GetStocks()).ReturnsAsync(stocksDict);
            List<Stocks> allStocks = stocksDict!
                .Select(temp => new Stocks()
                {
                    StockSymbol = temp["symbol"],
                    StockName = temp["description"]
                }).ToList();
            IActionResult result = await stocksController.Explore(null, true);

            // Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<Stocks>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(allStocks);
        }

        #endregion
    }
}
