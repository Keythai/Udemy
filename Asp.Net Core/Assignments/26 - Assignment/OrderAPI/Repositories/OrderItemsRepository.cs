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
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrdersRepository> _logger;
        public OrderItemsRepository(ApplicationDbContext context, ILogger<OrdersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<OrderItem> AddOrderItem(OrderItem orderItem)
        {
            _logger.LogInformation($"Adding new order with ID: {orderItem.OrderItemId}");
            _context.Add(orderItem);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order with ID: {orderItem.OrderItemId} added successfully.");
            return orderItem;
        }

        public async Task<bool> DeleteOrderItemById(Guid orderItemId)
        {
            _logger.LogInformation($"Deleting order with ID: {orderItemId}");
            var order = await _context.OrderItems.FindAsync(orderItemId);
            if (order == null)
            {
                _logger.LogWarning($"Order with ID: {orderItemId} not found.");
                return false;
            }
            _context.Remove(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order with ID: {orderItemId} deleted successfully.");
            return true;
        }

        public async Task<List<OrderItem>> GetAllOrderItems()
        {
            _logger.LogInformation("Fetching all orders.");
            _logger.LogInformation("All orders fetched successfully.");
            return await _context.OrderItems.ToListAsync();
        }

        public async Task<OrderItem> GetOrderItemByOrderItemId(Guid orderItemId)
        {
            _logger.LogInformation($"Fetching order with order item ID: {orderItemId}");
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem == null)
            {
                _logger.LogWarning($"Order with order item ID: {orderItemId} not found.");
                return null;
            }
            _logger.LogInformation($"Order with order item ID: {orderItemId} fetched successfully.");
            return orderItem;
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(Guid orderId)
        {
            _logger.LogInformation($"Fetching order items with order ID: {orderId}");
            List<OrderItem> orderItems = await _context.OrderItems.Where(o => o.OrderId == orderId).ToListAsync();
            if(orderItems.Count == 0)
            {
                _logger.LogInformation($"No order found from order ID: {orderId}");
                return null;
            }
            _logger.LogInformation($"Order items with order ID: {orderId} fetched successfully.");
            return orderItems;
        }

        public async Task<OrderItem> UpdateOrderItem(OrderItem orderItem)
        {
            _logger.LogInformation($"Updating order with ID: {orderItem.OrderItemId}");
            var existingOrderItem = await _context.OrderItems.FindAsync(orderItem.OrderItemId);
            if (existingOrderItem == null)
            {
                _logger.LogWarning($"Order with ID: {orderItem.OrderItemId} not found.");
                return null;
            }
            existingOrderItem.ProductName = orderItem.ProductName;
            existingOrderItem.Quantity = orderItem.Quantity;
            existingOrderItem.UnitPrice = orderItem.UnitPrice;
            existingOrderItem.TotalPrice = orderItem.TotalPrice;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order with ID: {orderItem.OrderItemId} updated successfully.");
            return orderItem;
        }
    }
}
