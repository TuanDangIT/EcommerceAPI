using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Repositories
{
    public interface IReturnRepository
    {
        Task CreateAsync(Return @return, CancellationToken cancellationToken = default);
        Task<Return?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<Return?> GetAsync(Guid returnId, CancellationToken cancellationToken = default, 
            params Func<IQueryable<Return>, IQueryable<Return>>[] includeActions);
        Task<Return?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default, 
            params Func<IQueryable<Return>, IQueryable<Return>>[] includeActions);
        Task UpdateAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid returnId, CancellationToken cancellationToken = default);
    }
}
