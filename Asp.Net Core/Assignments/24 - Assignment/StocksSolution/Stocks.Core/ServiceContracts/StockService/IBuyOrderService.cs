using ServiceContracts.DTO;
using System;

namespace ServiceContracts.StockService
{
    public interface IBuyOrderService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        Task<List<BuyOrderResponse>> GetBuyOrders();
    }
}
