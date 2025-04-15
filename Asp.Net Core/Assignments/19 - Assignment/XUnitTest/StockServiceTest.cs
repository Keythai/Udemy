using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace XUnitTest
{
    public class StockServiceTest
    {
        private readonly IStocksService _stockService;
        public StockServiceTest()
        {
            _stockService = new StocksService(null);
        }

        #region CreateBuyOrder

        //1. When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateBuyOrder_NullBuyOrderRequest_ThrownArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //2. When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateBuyOrder_QuantityLessThanMinimum_ThrownArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                Price = 1,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = buyOrderQuantity
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //3. When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(100001)] //passing parameters to the tet method
        public async Task CreateBuyOrder_QuantityMoreThanMaximum_ThrownArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                Price = 1,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = buyOrderQuantity
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //4. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateBuyOrder_PriceLessThanMinimum_ThrownArgumentException(double price)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                Price = price,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = 1
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //5. When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(10001)] //passing parameters to the tet method
        public async Task CreateBuyOrder_PriceMoreThanMaximum_ThrownArgumentException(double price)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                Price = price,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = 1
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol_ThrownArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                Price = 1,
                StockName = "StockName",
                StockSymbol = null,
                Quantity = 1
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_DateLessThanMinimum_ThrownArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                Price = 1,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = 1,
                DateAndTimeOfOrder = new DateTime(1999, 12, 31)
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //8. If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID(guid).
        [Fact]
        public async Task CreateBuyOrder_ValidBuyOrderRequest()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                Price = 1,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = 1,
                DateAndTimeOfOrder = new DateTime(2000, 1, 1)
            };

            //Act
            BuyOrderResponse buyOrderResponse = await _stockService.CreateBuyOrder(buyOrderRequest);
            List<BuyOrderResponse> allOrders = await _stockService.GetBuyOrders();

            //Assert
            Assert.NotEqual(Guid.Empty, buyOrderResponse.BuyOrderID);
            Assert.Contains(buyOrderResponse, allOrders);
        }

        #endregion

        #region CreateSellOrder

        //1. When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_NullSellOrderRequest_ThrownArgumentNullException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        //2. When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateSellOrder_QuantityLessThanMinimum_ThrownArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Price = 1,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = sellOrderQuantity
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        //3. When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateSellOrder_QuantityMoreThanMaximum_ThrownArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Price = 1,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = 100001
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        //4. When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateSellOrder_PriceLessThanMinimum_ThrownArgumentException(double price)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Price = price,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = 1
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        //5. When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(10001)] //passing parameters to the tet method
        public async Task CreateSellOrder_PriceMoreThanMaximum_ThrownArgumentException(double price)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Price = price,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = 1
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        //6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_NullStockSymbol_ThrownArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Price = 1,
                StockName = "StockName",
                StockSymbol = null,
                Quantity = 1
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        //7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_DateLessThanMinimum_ThrownArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Price = 1,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = 1,
                DateAndTimeOfOrder = new DateTime(1999, 12, 31)
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        //8. If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID(guid).
        [Fact]
        public async Task CreateSellOrder_ValidBuyOrderRequest()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Price = 1,
                StockName = "StockName",
                StockSymbol = "StockSymbol",
                Quantity = 1,
                DateAndTimeOfOrder = new DateTime(2000, 1, 1)
            };

            //Act
            SellOrderResponse sellOrderResponse = await _stockService.CreateSellOrder(sellOrderRequest);
            List<SellOrderResponse> allOrders = await _stockService.GetSellOrders();

            //Assert
            Assert.NotEqual(Guid.Empty, sellOrderResponse.SellOrderID);
            Assert.Contains(sellOrderResponse, allOrders);
        }

        #endregion

        #region GetAllBuyOrders

        //1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllBuyOrders_Default_ReturnEmptyList()
        {
            //Act
            List<BuyOrderResponse> allOrders = await _stockService.GetBuyOrders();
            //Assert
            Assert.Empty(allOrders);
        }

        //2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async Task GetAllBuyOrders_ValidBuyOrder()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest1 = new BuyOrderRequest()
            {
                Price = 1,
                StockName = "StockName1",
                StockSymbol = "StockSymbol1",
                Quantity = 1,
                DateAndTimeOfOrder = new DateTime(2000, 1, 1)
            };
            BuyOrderRequest? buyOrderRequest2 = new BuyOrderRequest()
            {
                Price = 1,
                StockName = "StockName2",
                StockSymbol = "StockSymbol2",
                Quantity = 1,
                DateAndTimeOfOrder = new DateTime(2001, 1, 1)
            };

            BuyOrderResponse buyOrderResponse1 = await _stockService.CreateBuyOrder(buyOrderRequest1);
            BuyOrderResponse buyOrderResponse2 = await _stockService.CreateBuyOrder(buyOrderRequest2);

            //Act
            List<BuyOrderResponse> buy_response_from_add = new List<BuyOrderResponse>() { buyOrderResponse1, buyOrderResponse2 };
            List<BuyOrderResponse> allOrders = await _stockService.GetBuyOrders();

            //Assert
            foreach (BuyOrderResponse items in buy_response_from_add)
            {
                Assert.Contains(items, allOrders);
            }
        }
        #endregion

        #region GetAllSellOrders

        //1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllSellOrders_Default_ReturnEmptyList()
        {
            //Act
            List<SellOrderResponse> allOrders = await _stockService.GetSellOrders();
            //Assert
            Assert.Empty(allOrders);
        }

        //2. When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async Task GetAllSellOrders_ValidBuyOrder()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest1 = new SellOrderRequest()
            {
                Price = 1,
                StockName = "StockName1",
                StockSymbol = "StockSymbol1",
                Quantity = 1,
                DateAndTimeOfOrder = new DateTime(2000, 1, 1)
            };
            SellOrderRequest? sellOrderRequest2 = new SellOrderRequest()
            {
                Price = 1,
                StockName = "StockName2",
                StockSymbol = "StockSymbol2",
                Quantity = 1,
                DateAndTimeOfOrder = new DateTime(2001, 1, 1)
            };

            SellOrderResponse sellOrderResponse1 = await _stockService.CreateSellOrder(sellOrderRequest1);
            SellOrderResponse sellOrderResponse2 = await _stockService.CreateSellOrder(sellOrderRequest2);

            //Act
            List<SellOrderResponse> sell_response_from_add = new List<SellOrderResponse>() { sellOrderResponse1, sellOrderResponse2 };
            List<SellOrderResponse> allOrders = await _stockService.GetSellOrders();

            //Assert
            foreach (SellOrderResponse items in sell_response_from_add)
            {
                Assert.Contains(items, allOrders);
            }
        }
        #endregion
    }
}
