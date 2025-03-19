using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Repositories
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly OrdersDbContext _dbContext;

        public OrderRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(order, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Order?> GetAsync(Guid orderId, CancellationToken cancellationToken= default, 
            params Func<IQueryable<Order>, IQueryable<Order>>[] includeActions)
        {
            var query = _dbContext.Orders
                .AsQueryable();
            if(includeActions is not null)
            {
                foreach(var includeAction in includeActions)
                {
                    query = includeAction(query);
                }
            }
            var order = await query
                .FirstOrDefaultAsync(o => o.Id ==  orderId, cancellationToken);
            return order;
        }

        public async Task<Order?> GetAsync(string trackingNumber, CancellationToken cancellationToken = default)
            => await _dbContext.Orders
                .Include(o => o.Shipments)
                .FirstOrDefaultAsync(o => o.Shipments.Select(s => s.TrackingNumber).Contains(trackingNumber), cancellationToken);

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);

        public async Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default)
            => await _dbContext.Orders
                .Where(o => o.Id == orderId)
                .ExecuteDeleteAsync(cancellationToken);
    }
}
