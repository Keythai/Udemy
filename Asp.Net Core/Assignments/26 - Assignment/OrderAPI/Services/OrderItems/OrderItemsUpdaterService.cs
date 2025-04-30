using ServiceContracts.DTO;
using ServiceContracts.OrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderItems
{
    public class OrderItemsUpdaterService : IOrderItemsUpdaterService
    {
        public Task<OrderItemResponse> UpdateOrderItem(OrderItemUpdateRequest orderItemRequest)
        {
            throw new NotImplementedException();
        }
    }
}
