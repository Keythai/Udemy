using Entities;

namespace Repositories
{
    public interface IStocksRepository
    {
        /// <summary>
        /// Creates a new buy order in the database.
        /// </summary>
        /// <param name="buyOrder">buy order to add</param>
        /// <returns>Return buy order that has been added</returns>
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);
        /// <summary>
        /// Creates a new sell order in the database.
        /// </summary>
        /// <param name="sellOrder">sell order to add</param>
        /// <returns>Return sell order that has been added</returns>
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);
        /// <summary>
        /// Gets all the buy orders from the database.
        /// </summary>
        /// <returns>List of buy orders</returns>
        Task<List<BuyOrder>> GetBuyOrders();
        /// <summary>
        /// Gets all the sell orders from the database.
        /// </summary>
        /// <returns>List of sell orders</returns>
        Task<List<SellOrder>> GetSellOrders();
    }
}
