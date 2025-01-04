using Ecommerce.Modules.Inventory.Application.DAL;
using Ecommerce.Shared.Infrastructure.Postgres;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.UnitOfWork
{
    internal class InventoryUnitOfWork : PostgresUnitOfWork<InventoryDbContext>, IInventoryUnitOfWork
    {
        public InventoryUnitOfWork(InventoryDbContext dbContext) : base(dbContext)
        {
        }
    }
}
