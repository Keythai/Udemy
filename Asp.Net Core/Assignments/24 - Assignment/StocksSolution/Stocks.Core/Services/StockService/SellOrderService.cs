using Entities;
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
    public class SellOrderService : ISellOrderService
    {
        private readonly IStocksRepository _stocksRepository;
        public SellOrderService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }
        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if(sellOrderRequest == null)
            {
                throw new ArgumentNullException();
            }
            ValidationHelper.ModelValidation(sellOrderRequest);
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            await _stocksRepository.CreateSellOrder(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            return (await _stocksRepository.GetSellOrders())
                .Select(x => x.ToSellOrderResponse()).ToList();
        }
    }
}
