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
        public async Task CreateAsync(Order order)
        {
            await _dbContext.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Order?> GetAsync(Guid orderId)
            => await _dbContext.Orders
                .Include(o => o.Products)
                .Include(o => o.Shipments)
                .ThenInclude(s => s.Parcels)
                .Include(o => o.Shipments)
                .ThenInclude(s => s.Receiver)
                .ThenInclude(r => r.Address)
                .Include(o => o.Customer)
                .Include(o => o.Invoice)
                .SingleOrDefaultAsync(o => o.Id == orderId);

        public async Task<Order?> GetAsync(string trackingNumber)
            => await _dbContext.Orders
                .Include(o => o.Shipments)
                .SingleOrDefaultAsync(o => o.Shipments.Select(s => s.TrackingNumber).Contains(trackingNumber));

        public async Task UpdateAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
