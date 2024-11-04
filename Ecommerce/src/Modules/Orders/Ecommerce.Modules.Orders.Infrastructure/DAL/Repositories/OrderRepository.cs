using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Modules.Orders.Domain.Shipping.Entities;
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
        public async Task CreateOrderAsync(Order order)
        {
            await _dbContext.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderAsync(Guid orderId)
            => await _dbContext.Orders
                .Include(o => o.Products)
                .Include(o => o.Shipment)
                .ThenInclude(s => s.Parcels)
                .Include(o => o.Shipment)
                .ThenInclude(s => s.Receiver)
                .ThenInclude(r => r.Address)
                .Include(o => o.Customer)
                .Include(o => o.Invoice)
                .SingleOrDefaultAsync(o => o.Id == orderId);

        public async Task<Order?> GetOrderAsync(string trackingNumber)
            => await _dbContext.Orders
                .Include(o => o.Shipment)
                .SingleOrDefaultAsync(o => o.Shipment.TrackingNumber == trackingNumber);

        public async Task UpdateAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
