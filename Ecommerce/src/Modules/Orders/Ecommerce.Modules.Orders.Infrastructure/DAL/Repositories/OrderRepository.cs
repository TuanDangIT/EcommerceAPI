using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
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
        public async Task CreateOrder(Order order)
        {
            await _dbContext.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}
