using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Repositories
{
    internal class ReturnRepository : IReturnRepository
    {
        private readonly OrdersDbContext _dbContext;

        public ReturnRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(Return @return, CancellationToken cancellationToken = default)
        {
            await _dbContext.Returns.AddAsync(@return, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid returnId, CancellationToken cancellationToken = default)
            => await _dbContext.Returns
                .Where(r => r.Id == returnId)
                .ExecuteDeleteAsync(cancellationToken);

        public async Task<Return?> GetAsync(Guid returnId, CancellationToken cancellationToken = default, params Func<IQueryable<Return>, IQueryable<Return>>[] includeActions)
        {
            var query = _dbContext.Returns
                .AsQueryable();
            if(includeActions is not null)
            {
                foreach(var includeAction in includeActions)
                {
                    query = includeAction(query);
                }
            }
            var @return = await query
                .FirstOrDefaultAsync(r => r.Id == returnId, cancellationToken);
            return @return;
        }
        public async Task<Return?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default, params Func<IQueryable<Return>, IQueryable<Return>>[] includeActions)
        {
            var query = _dbContext.Returns
                .Include(r => r.Order)
                .AsQueryable();
            if (includeActions is not null)
            {
                foreach (var includeAction in includeActions)
                {
                    query = includeAction(query);
                }
            }
            var @return = await query
                .FirstOrDefaultAsync(r => r.OrderId == orderId, cancellationToken);
            return @return;
        }

        public async Task<Return?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
            => await _dbContext.Returns
                .Include(r => r.Order)
                .FirstOrDefaultAsync(r => r.Order.Id == orderId, cancellationToken);

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
