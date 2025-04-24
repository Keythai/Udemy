using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.StockService;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StockService
{
    public class BuyOrderService : IBuyOrderService
    {
        private readonly IStocksRepository _stocksRepository;
        public BuyOrderService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if(buyOrderRequest == null)
            {
                throw new ArgumentNullException();
            }
            ValidationHelper.ModelValidation(buyOrderRequest);
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();

            await _stocksRepository.CreateBuyOrder(buyOrder);

            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return (await _stocksRepository.GetBuyOrders())
                .Select(x => x.ToBuyOrderResponse()).ToList();
        }

    }
}
