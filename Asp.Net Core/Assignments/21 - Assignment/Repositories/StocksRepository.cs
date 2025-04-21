using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace RepositoryContracts
{
    public class StocksRepository : IStocksRepository
    {
        private readonly ApplicationDbContext _context;
        public StocksRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _context.BuyOrders.Add(buyOrder);
            await _context.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _context.SellOrders.Add(sellOrder);
            await _context.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            List<BuyOrder> buyOrders = await _context.BuyOrders
                .OrderByDescending(temp => temp.DateAndTimeOfOrder)
                .ToListAsync();
            return buyOrders;
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _context.SellOrders
                .OrderByDescending(temp => temp.DateAndTimeOfOrder)
                .ToListAsync();
            return sellOrders;
        }
    }
}
