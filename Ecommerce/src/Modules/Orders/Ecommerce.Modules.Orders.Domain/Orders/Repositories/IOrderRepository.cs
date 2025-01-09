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
        Task CreateAsync(Order order, CancellationToken cancellationToken = default);
        Task<Order?> GetAsync(Guid orderId, CancellationToken cancellationToken = default, params Func<IQueryable<Order>, IQueryable<Order>>[] includeActions);
        Task<Order?> GetAsync(string trackingNumber, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
    }
}
