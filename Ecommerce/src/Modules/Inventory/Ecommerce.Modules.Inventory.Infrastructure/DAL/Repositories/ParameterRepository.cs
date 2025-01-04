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
        public async Task AddAsync(Parameter parameter, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(parameter, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddManyAsync(IEnumerable<Parameter> parameters, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddRangeAsync(parameters, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid parameterId, CancellationToken cancellationToken = default) 
            => await _dbContext.Parameters.Where(p => p.Id == parameterId).ExecuteDeleteAsync(cancellationToken);

        public async Task DeleteManyAsync(CancellationToken cancellationToken = default, params Guid[] parameterIds) 
            => await _dbContext.Parameters.Where(p => parameterIds.Contains(p.Id)).ExecuteDeleteAsync(cancellationToken);

        public async Task<IEnumerable<Parameter>> GetAllAsync(CancellationToken cancellationToken = default) 
            => await _dbContext.Parameters.ToListAsync(cancellationToken);

        public async Task<Parameter?> GetAsync(Guid parameterId, CancellationToken cancellationToken = default) 
            => await _dbContext.Parameters.SingleOrDefaultAsync(p => p.Id == parameterId, cancellationToken);

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
