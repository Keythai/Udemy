using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Orders
{
    public class OrdersAdderService : IOrdersAdderService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderItemsRepository _orderItemsRepository;
        private readonly ILogger<OrdersAdderService> _logger;
        public OrdersAdderService(IOrdersRepository ordersRepository, IOrderItemsRepository orderItemsRepository, ILogger<OrdersAdderService> logger)
        {
            _ordersRepository = ordersRepository;
            _orderItemsRepository = orderItemsRepository;
            _logger = logger;
        }
        public async Task<OrderResponse> AddOrder(OrderAddRequest orderRequest)
        {
            _logger.LogInformation($"Adding a new order.");
            var order = orderRequest.ToOrder();
            order.OrderId = Guid.NewGuid();
            await _ordersRepository.AddOrder(order);

            if (orderRequest.OrderItems == null || !orderRequest.OrderItems.Any())
            {
                _logger.LogWarning("Order items are null or empty.");
                throw new ArgumentException("Order items cannot be null or empty.");
            }
            var orderItems = orderRequest.OrderItems.Select(item => item.ToOrderItem()).ToList();
            var orderResponse = order.ToOrderResponse();
            foreach (var item in orderItems)
            {
                item.OrderItemId = Guid.NewGuid();
                item.OrderId = order.OrderId;

                await _orderItemsRepository.AddOrderItem(item);
                orderResponse.OrderItems.Add(item.ToOrderItemResponse());
            }
            _logger.LogInformation($"Order with ID: {order.OrderId} added successfully.");
            return orderResponse;
        }
    }
}
