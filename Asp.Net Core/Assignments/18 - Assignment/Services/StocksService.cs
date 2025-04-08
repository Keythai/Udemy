using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly StockMarketDbContext _context;
        public StocksService(StockMarketDbContext context)
        {
            _context = context;
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

            _context.BuyOrders.Add(buyOrder);
            await _context.SaveChangesAsync();

            return buyOrder.ToBuyOrderResponse();
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

            _context.SellOrders.Add(sellOrder);
            await _context.SaveChangesAsync();

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return await _context.BuyOrders.Select(x => x.ToBuyOrderResponse()).ToListAsync();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            return await _context.SellOrders.Select(x => x.ToSellOrderResponse()).ToListAsync();
        }
    }
}
