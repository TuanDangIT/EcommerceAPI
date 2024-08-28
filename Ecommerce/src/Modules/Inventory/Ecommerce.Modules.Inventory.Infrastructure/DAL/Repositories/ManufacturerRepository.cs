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
        public async Task<int> AddAsync(Manufacturer manufacturer)
        {
            await _dbContext.AddAsync(manufacturer);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid manufacturerId) => await _dbContext.Manufacturers.Where(m => m.Id == manufacturerId).ExecuteDeleteAsync();

        public async Task<int> DeleteManyAsync(params Guid[] manufacturerIds) => await _dbContext.Manufacturers.Where(m => manufacturerIds.Contains(m.Id)).ExecuteDeleteAsync();

        public async Task<IEnumerable<Manufacturer>> GetAllAsync() => await _dbContext.Manufacturers.ToListAsync();

        public Task<Manufacturer?> GetAsync(Guid id) => _dbContext.Manufacturers.SingleOrDefaultAsync(m => m.Id == id); 

        public async Task<int> UpdateAsync(Manufacturer manufacturer)
        {
            _dbContext.Manufacturers.Update(manufacturer);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
