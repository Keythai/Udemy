using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.OrderItems
{
    public interface IOrderItemsGetterService
    {
        Task<OrderItemResponse?> GetOrderItemByOrderItemId(Guid orderItemId);
        Task<List<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId);
        Task<List<OrderItemResponse>> GetAllOrderItems();
    }
}
