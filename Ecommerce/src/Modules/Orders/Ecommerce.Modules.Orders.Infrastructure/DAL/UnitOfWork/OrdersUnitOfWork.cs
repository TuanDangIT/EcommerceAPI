using Ecommerce.Modules.Orders.Application.Shared.UnitOfWork;
using Ecommerce.Shared.Infrastructure.Postgres;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.UnitOfWork
{
    internal class OrdersUnitOfWork : PostgresUnitOfWork<OrdersDbContext>, IOrdersUnitOfWork
    {
        public OrdersUnitOfWork(OrdersDbContext dbContext) : base(dbContext)
        {
        }
    }
}
