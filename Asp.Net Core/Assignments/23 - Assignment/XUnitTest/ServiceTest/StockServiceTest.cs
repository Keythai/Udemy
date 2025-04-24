using ServiceContracts.DTO;
using Entities;
using Repositories;
using Moq;
using AutoFixture;
using FluentAssertions;
using ServiceContracts.StockService;
using Services.StockService;

namespace XUnitTest
{
    public class StockServiceTest
    {
        private readonly IBuyOrderService _buyOrderService;
        private readonly ISellOrderService _sellOrderService;
        private readonly IStocksRepository _stocksRepository;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly IFixture _fixture;
        public StockServiceTest()
        {
            _fixture = new Fixture();
            
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;

            _buyOrderService = new BuyOrderService(_stocksRepository);
            _sellOrderService = new SellOrderService(_stocksRepository);
        }

        #region CreateBuyOrder

        //1. When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateBuyOrder_NullBuyOrderRequest_ThrownArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;
            BuyOrder buyOrderFixture = _fixture.Create<BuyOrder>();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrderFixture);

            //Act
            Func<Task> action = (async () =>
            {
                await _buyOrderService.CreateBuyOrder(buyOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //2. When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateBuyOrder_QuantityLessThanMinimum_ThrownArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, buyOrderQuantity)
                .Create();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = (async () =>
            {
                await _buyOrderService.CreateBuyOrder(buyOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //3. When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(100001)] //passing parameters to the tet method
        public async Task CreateBuyOrder_QuantityMoreThanMaximum_ThrownArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, buyOrderQuantity)
                .Create();
            BuyOrder buyOrderFixture = _fixture.Create<BuyOrder>();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrderFixture);

            //Act
            Func<Task> action = (async () =>
            {
                await _buyOrderService.CreateBuyOrder(buyOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //4. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateBuyOrder_PriceLessThanMinimum_ThrownArgumentException(double price)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, price)
                .Create();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = (async () =>
            {
                await _buyOrderService.CreateBuyOrder(buyOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //5. When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(10001)] //passing parameters to the tet method
        public async Task CreateBuyOrder_PriceMoreThanMaximum_ThrownArgumentException(double price)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, price)
                .Create();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = (async () =>
            {
                await _buyOrderService.CreateBuyOrder(buyOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol_ThrownArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.StockSymbol, null as string)
             .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _buyOrderService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_DateLessThanMinimum_ThrownArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
             .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = (async () =>
            {
                await _buyOrderService.CreateBuyOrder(buyOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //8. If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID(guid).
        [Fact]
        public async Task CreateBuyOrder_ValidBuyOrderRequest()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            BuyOrderResponse buyOrderResponseActual = await _buyOrderService.CreateBuyOrder(buyOrderRequest);
            buyOrder.BuyOrderID = buyOrderResponseActual.BuyOrderID;
            BuyOrderResponse buyOrderResponseExpected = buyOrder.ToBuyOrderResponse();

            //Assert
            buyOrderResponseActual.BuyOrderID.Should().NotBe(Guid.Empty);
            buyOrderResponseExpected.Should().Be(buyOrderResponseActual);
        }

        #endregion

        #region CreateSellOrder

        //1. When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_NullSellOrderRequest_ThrownArgumentNullException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            SellOrder sellOrderFixture = _fixture.Create<SellOrder>();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrderFixture);

            //Act
            Func<Task> action = (async () =>
            {
                await _sellOrderService.CreateSellOrder(sellOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //2. When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateSellOrder_QuantityLessThanMinimum_ThrownArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, sellOrderQuantity)
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = (async () =>
            {
                await _sellOrderService.CreateSellOrder(sellOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //3. When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateSellOrder_QuantityMoreThanMaximum_ThrownArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, sellOrderQuantity)
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = (async () =>
            {
                await _sellOrderService.CreateSellOrder(sellOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //4. When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateSellOrder_PriceLessThanMinimum_ThrownArgumentException(double price)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, price)
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = (async () =>
            {
                await _sellOrderService.CreateSellOrder(sellOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //5. When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Theory] //Use [Theory] instead of [Fact]; so that, you can pass parameters to the test method
        [InlineData(10001)] //passing parameters to the tet method
        public async Task CreateSellOrder_PriceMoreThanMaximum_ThrownArgumentException(double price)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, price)
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = (async () =>
            {
                await _sellOrderService.CreateSellOrder(sellOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_NullStockSymbol_ThrownArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = (async () =>
            {
                await _sellOrderService.CreateSellOrder(sellOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_DateLessThanMinimum_ThrownArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = (async () =>
            {
                await _sellOrderService.CreateSellOrder(sellOrderRequest);
            });

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //8. If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID(guid).
        [Fact]
        public async Task CreateSellOrder_ValidBuyOrderRequest()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .Create();

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            //Act
            SellOrderResponse sellOrderResponseActual = await _sellOrderService.CreateSellOrder(sellOrderRequest);
            sellOrder.SellOrderID = sellOrderResponseActual.SellOrderID;
            SellOrderResponse sellOrderResponseExpected = sellOrder.ToSellOrderResponse();

            //Assert
            sellOrderResponseActual.SellOrderID.Should().NotBe(Guid.Empty);
            sellOrderResponseActual.Should().Be(sellOrderResponseExpected);
        }

        #endregion

        #region GetAllBuyOrders

        //1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllBuyOrders_Default_ReturnEmptyList()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>();
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrders);

            //Act
            List<BuyOrderResponse> allOrders = await _buyOrderService.GetBuyOrders();
            //Assert
            allOrders.Should().BeEmpty();
        }

        //2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async Task GetAllBuyOrders_ValidBuyOrder()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>()
            {
                _fixture.Create<BuyOrder>(),
                _fixture.Create<BuyOrder>()
            };

            //Act
            List<BuyOrderResponse> buyOrderResponseListExpected = buyOrders.Select(x => x.ToBuyOrderResponse()).ToList();
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrders);
            List<BuyOrderResponse> allOrders = await _buyOrderService.GetBuyOrders();

            //Assert
            allOrders.Should().BeEquivalentTo(buyOrderResponseListExpected);
        }
        #endregion

        #region GetAllSellOrders

        //1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllSellOrders_Default_ReturnEmptyList()
        {
            //Arrange
            List<SellOrder> sellOrders = new List<SellOrder>();
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(sellOrders);

            //Act
            List<SellOrderResponse> allOrders = await _sellOrderService.GetSellOrders();
            //Assert
            Assert.Empty(allOrders);
        }

        //2. When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async Task GetAllSellOrders_ValidBuyOrder()
        {
            //Arrange
            List<SellOrder> sellOrders = new List<SellOrder>()
            {
                _fixture.Create<SellOrder>(),
                _fixture.Create<SellOrder>()
            };

            //Act
            List<SellOrderResponse> sellOrdersListExpected = sellOrders.Select(x => x.ToSellOrderResponse()).ToList();
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(sellOrders);
            List<SellOrderResponse> allOrders = await _sellOrderService.GetSellOrders();

            //Assert
            allOrders.Should().BeEquivalentTo(sellOrdersListExpected);
        }
        #endregion
    }
}
