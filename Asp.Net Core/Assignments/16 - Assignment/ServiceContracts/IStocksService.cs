﻿using ServiceContracts.DTO;
using System;

namespace ServiceContracts
{
    public interface IStocksService
    {
        BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest);
        List<BuyOrderResponse> GetBuyOrders();
        List<SellOrderResponse> GetSellOrders();
    }
}
