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
    internal class ShipmentRepository : IShipmentRepository
    {
        private readonly OrdersDbContext _dbContext;

        public ShipmentRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Shipment?> GetAsync(int shipmentId, CancellationToken cancellationToken = default)
            => await _dbContext.Shipments
                .SingleOrDefaultAsync(s => s.Id == shipmentId, cancellationToken);
    }
}
