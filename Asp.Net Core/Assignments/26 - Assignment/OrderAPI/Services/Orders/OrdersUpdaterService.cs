using Microsoft.Extensions.Logging;
using OrderAPI.Models;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.OrderItems;
using ServiceContracts.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Orders
{
    public class OrdersUpdaterService : IOrdersUpdaterService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger<OrdersAdderService> _logger;
        public OrdersUpdaterService(IOrdersRepository ordersRepository, ILogger<OrdersAdderService> logger)
        {
            _ordersRepository = ordersRepository;
            _logger = logger;
        }
        public async Task<OrderResponse> UpdateOrder(OrderUpdateRequest orderRequest)
        {
            _logger.LogInformation($"Updating order with ID: {orderRequest.OrderId}");
            var order = orderRequest.ToOrder();
            var updatedOrder = await _ordersRepository.UpdateOrder(order);
            var orderResponse = updatedOrder.ToOrderResponse();
            
            _logger.LogInformation($"Order with ID: {orderRequest.OrderId} updated successfully.");
            return orderResponse;
        }
    }
}
