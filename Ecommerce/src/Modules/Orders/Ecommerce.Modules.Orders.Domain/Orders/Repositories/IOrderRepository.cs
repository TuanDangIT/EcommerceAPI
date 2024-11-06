using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Repositories
{
    public interface IOrderRepository
    {
        Task CreateAsync(Order order);
        Task<Order?> GetAsync(Guid orderId);
        Task<Order?> GetAsync(string trackingNumber);
        Task UpdateAsync();
    }
}
