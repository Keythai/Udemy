using OrderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IOrdersRepository
    {
        Task<Order> AddOrder(Order order);
        Task<bool> DeleteOrderById(Guid orderId);
        Task<List<Order>> GetAllOrders();
        Task<Order> UpdateOrder(Order order);
        Task<Order> GetOrderById(Guid orderId);
        Task<List<Order>> GetFilteredOrders(Expression<Func<Order, bool>> predicate);
    }
}
