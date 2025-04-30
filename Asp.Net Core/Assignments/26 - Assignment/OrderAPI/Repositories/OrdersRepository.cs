using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrdersRepository> _logger;
        public OrdersRepository(ApplicationDbContext context, ILogger<OrdersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Order> AddOrder(Order order)
        {
            _logger.LogInformation($"Adding new order with ID: {order.OrderId}");
            _context.Add(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order with ID: {order.OrderId} added successfully.");
            return order;
        }

        public async Task<bool> DeleteOrderById(Guid orderId)
        {
            _logger.LogInformation($"Deleting order with ID: {orderId}");
            var order = await _context.Orders.FindAsync(orderId);
            if(order == null)
            {
                _logger.LogWarning($"Order with ID: {orderId} not found.");
                return false;
            }
            _context.Remove(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order with ID: {orderId} deleted successfully.");
            return true;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            _logger.LogInformation("Fetching all orders.");
            _logger.LogInformation("All orders fetched successfully.");
            return await _context.Orders.ToListAsync();
        }

        public async Task<List<Order>> GetFilteredOrders(Expression<Func<Order, bool>> predicate)
        {
            _logger.LogInformation("Fetching filtered orders.");
            _logger.LogInformation("Filtered orders fetched successfully.");
            return await _context.Orders
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            _logger.LogInformation($"Fetching order with ID: {orderId}");
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning($"Order with ID: {orderId} not found.");
                return null;
            }
            _logger.LogInformation($"Order with ID: {orderId} fetched successfully.");
            return order;
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            _logger.LogInformation($"Updating order with ID: {order.OrderId}");
            var existingOrder = await _context.Orders.FindAsync(order.OrderId);
            if (existingOrder == null)
            {
                _logger.LogWarning($"Order with ID: {order.OrderId} not found.");
                return null;
            }
            existingOrder.OrderNumber = order.OrderNumber;
            existingOrder.CustomerName = order.CustomerName;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.TotalAmount = order.TotalAmount;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order with ID: {order.OrderId} updated successfully.");
            return order;
        }
    }
}
