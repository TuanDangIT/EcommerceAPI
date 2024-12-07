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

        public async Task<Return?> GetAsync(Guid returnId, CancellationToken cancellationToken = default)
            => await _dbContext.Returns
                .Include(r => r.Order)
                .ThenInclude(o => o.Customer)
                .SingleOrDefaultAsync(r => r.Id == returnId, cancellationToken);

        public async Task<Return?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
            => await _dbContext.Returns
                .Include(r => r.Order)
                .SingleOrDefaultAsync(r => r.Order.Id == orderId, cancellationToken);

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
