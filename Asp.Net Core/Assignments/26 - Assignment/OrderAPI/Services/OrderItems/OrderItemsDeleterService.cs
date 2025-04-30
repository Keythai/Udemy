using ServiceContracts.OrderItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderItems
{
    public class OrderItemsDeleterService : IOrderItemsDeleterService
    {
        public Task<bool> DeleteOrderItemById(Guid orderItemId)
        {
            throw new NotImplementedException();
        }
    }
}
