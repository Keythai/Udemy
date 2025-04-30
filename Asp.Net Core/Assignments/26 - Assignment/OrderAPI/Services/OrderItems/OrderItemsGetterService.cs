using ServiceContracts.DTO;
using ServiceContracts.OrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderItems
{
    public class OrderItemsGetterService : IOrderItemsGetterService
    {
        public Task<List<OrderItemResponse>> GetAllOrderItems()
        {
            throw new NotImplementedException();
        }

        public Task<OrderItemResponse?> GetOrderItemByOrderItemId(Guid orderItemId)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
