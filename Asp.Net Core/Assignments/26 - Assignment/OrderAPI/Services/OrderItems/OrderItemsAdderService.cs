using ServiceContracts.DTO;
using ServiceContracts.OrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderItems
{
    public class OrderItemsAdderService : IOrderItemsAdderService
    {
        public Task<OrderItemResponse> AddOrderItem(OrderItemAddRequest orderItemRequest)
        {
            throw new NotImplementedException();
        }
    }
}
