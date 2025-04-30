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
    public class OrdersGetterService : IOrdersGetterService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderItemsGetterService _orderItemsGetterService;
        private readonly ILogger<OrdersAdderService> _logger;
        public OrdersGetterService(IOrdersRepository ordersRepository, IOrderItemsGetterService orderItemsGetterService, ILogger<OrdersAdderService> logger)
        {
            _ordersRepository = ordersRepository;
            _orderItemsGetterService = orderItemsGetterService;
            _logger = logger;
        }
        public async Task<OrderResponse?> GetOrderById(Guid orderId)
        {
            _logger.LogInformation($"Fetching order with ID: {orderId}");
            var order = await _ordersRepository.GetOrderById(orderId);
            var orderResponse = order?.ToOrderResponse();
            if (orderResponse == null)
            {
                _logger.LogWarning($"Order with ID: {orderId} not found.");
            }
            _logger.LogInformation($"Order with ID: {orderId} fetched successfully.");
            return orderResponse;
        }
        public async Task<List<OrderResponse>> GetAllOrders()
        {
            _logger.LogInformation("Fetching all orders.");
            var orderResponseList = (await _ordersRepository.GetAllOrders()).ToOrderResponseList();
            foreach (var order in orderResponseList)
            {
                order.OrderItems = await _orderItemsGetterService.GetOrderItemsByOrderId(order.OrderId);
            }
            _logger.LogInformation("All orders fetched successfully.");
            return orderResponseList;
        }
        public async Task<List<OrderResponse>> GetFilteredOrders(string searchBy, string? searchString)
        {
            _logger.LogInformation($"Fetching filtered orders by {searchBy} with search string: {searchString}");
            List<Order> filteredOrders;
            switch(searchBy)
            {
                case nameof(Order.OrderId):
                    filteredOrders = await _ordersRepository.GetFilteredOrders(o => o.OrderId.ToString().Contains(searchString));
                    break;
                case nameof(Order.CustomerName):
                    filteredOrders = await _ordersRepository.GetFilteredOrders(o => o.CustomerName.Contains(searchString));
                    break;
                case nameof(Order.OrderNumber):
                    filteredOrders = await _ordersRepository.GetFilteredOrders(o => o.OrderNumber.ToString().Contains(searchString));
                    break;
                case nameof(Order.OrderDate):
                    filteredOrders = await _ordersRepository.GetFilteredOrders(o => o.OrderDate.ToString().Contains(searchString));
                    break;
                case nameof(Order.TotalAmount):
                    filteredOrders = await _ordersRepository.GetFilteredOrders(o => o.TotalAmount.ToString().Contains(searchString));
                    break;
                default:
                    _logger.LogWarning($"Invalid search criteria: {searchBy}");
                    return new List<OrderResponse>();
            }

            _logger.LogInformation($"Filtered orders fetched successfully.");
            return filteredOrders.ToOrderResponseList();
        }
    }
}
