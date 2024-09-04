using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Repositories
{
    internal class ParameterRepository : IParameterRepository
    {
        private readonly InventoryDbContext _dbContext;

        public ParameterRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Parameter parameter)
        {
            await _dbContext.AddAsync(parameter);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid parameterId) => await _dbContext.Parameters.Where(p => p.Id == parameterId).ExecuteDeleteAsync();

        public async Task DeleteManyAsync(params Guid[] parameterIds) => await _dbContext.Parameters.Where(p => parameterIds.Contains(p.Id)).ExecuteDeleteAsync();

        public async Task<IEnumerable<Parameter>> GetAllAsync() => await _dbContext.Parameters.ToListAsync();

        public async Task<Parameter?> GetAsync(Guid parameterId) => await _dbContext.Parameters.SingleOrDefaultAsync(p => p.Id == parameterId);

        public async Task UpdateAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
