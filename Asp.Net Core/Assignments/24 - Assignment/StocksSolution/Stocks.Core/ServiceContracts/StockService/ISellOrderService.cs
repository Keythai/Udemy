using ServiceContracts.DTO;
using System;

namespace ServiceContracts.StockService
{
    public interface ISellOrderService
    {
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
