using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Orders
{
    public interface IOrdersDeleterService
    {
        Task<bool> DeleteOrderById(Guid orderId);
    }
}
