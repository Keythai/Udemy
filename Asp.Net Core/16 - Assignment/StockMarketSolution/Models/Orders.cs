﻿using ServiceContracts.DTO;

namespace StockMarketSolution.Models
{
    public class Orders
    {
        public List<BuyOrderResponse>? BuyOrders { get; set; }
        public List<SellOrderResponse>? SellOrders { get; set; }
    }
}
