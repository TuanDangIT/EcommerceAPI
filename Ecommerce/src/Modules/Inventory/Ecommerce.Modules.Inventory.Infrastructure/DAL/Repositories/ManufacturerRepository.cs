using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Modules.Inventory.Domain;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Repositories
{
    internal class ManufacturerRepository : IManufacturerRepository
    {
        private readonly InventoryDbContext _dbContext;

        public ManufacturerRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Manufacturer manufacturer, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(manufacturer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddManyAsync(IEnumerable<Manufacturer> manufacturers, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddRangeAsync(manufacturers, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid manufacturerId, CancellationToken cancellationToken = default) 
            => await _dbContext.Manufacturers.Where(m => m.Id == manufacturerId).ExecuteDeleteAsync(cancellationToken);

        public async Task DeleteManyAsync(CancellationToken cancellationToken = default, params Guid[] manufacturerIds) 
            => await _dbContext.Manufacturers.Where(m => manufacturerIds.Contains(m.Id)).ExecuteDeleteAsync(cancellationToken);

        public async Task<IEnumerable<Manufacturer>> GetAllAsync(CancellationToken cancellationToken = default) 
            => await _dbContext.Manufacturers.ToListAsync(cancellationToken);

        public Task<Manufacturer?> GetAsync(Guid id, CancellationToken cancellationToken = default) 
            => _dbContext.Manufacturers.SingleOrDefaultAsync(m => m.Id == id, cancellationToken); 

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
