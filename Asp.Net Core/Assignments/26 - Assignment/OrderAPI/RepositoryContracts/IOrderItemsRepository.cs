using OrderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IOrderItemsRepository
    {
        Task<OrderItem> AddOrderItem(OrderItem orderItem);
        Task<bool> DeleteOrderItemById(Guid orderItemId);
        Task<List<OrderItem>> GetAllOrderItems();
        Task<OrderItem> UpdateOrderItem(OrderItem orderItem);
        Task<OrderItem> GetOrderItemByOrderItemId(Guid orderItemId);
        Task<List<OrderItem>> GetOrderItemsByOrderId(Guid orderId);
    }
}
