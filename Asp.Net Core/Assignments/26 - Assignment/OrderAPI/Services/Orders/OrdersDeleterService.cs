using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Orders
{
    public class OrdersDeleterService : IOrdersDeleterService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger<OrdersAdderService> _logger;
        public OrdersDeleterService(IOrdersRepository ordersRepository, ILogger<OrdersAdderService> logger)
        {
            _ordersRepository = ordersRepository;
            _logger = logger;
        }
        public async Task<bool> DeleteOrderById(Guid orderId)
        {
            _logger.LogInformation($"Deleting order with ID: {orderId}");

            var isDeleted = await _ordersRepository.DeleteOrderById(orderId);
            if (isDeleted)
            {
                _logger.LogInformation($"Order with ID: {orderId} deleted successfully.");
            }
            else
            {
                _logger.LogWarning($"Order with ID: {orderId} not found.");
            }
            return isDeleted;
        }
    }
}
