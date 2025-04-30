using OrderAPI.Models;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Orders
{
    public interface IOrdersGetterService
    {
        Task<OrderResponse?> GetOrderById(Guid orderId);
        Task<List<OrderResponse>> GetAllOrders();
        Task<List<OrderResponse>> GetFilteredOrders(string searchBy, string? searchString);
    }
}
