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
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly InventoryDbContext _dbContext;

        public CategoryRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(category, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task AddManyAsync(IEnumerable<Category> categories, CancellationToken cancellationToken = default)
        {
            var existingCategoryNames = await _dbContext.Categories
                                                        .Where(c => categories.Select(x => x.Name).Contains(c.Name))
                                                        .Select(c => c.Name)
                                                        .ToListAsync(cancellationToken);

            var categoriesToAdd = categories.Where(c => !existingCategoryNames.Contains(c.Name)).ToList();

            if (categoriesToAdd.Any())
            {
                await _dbContext.AddRangeAsync(categoriesToAdd, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default) 
            => await _dbContext.Categories.Where(c => c.Id == categoryId).ExecuteDeleteAsync(cancellationToken);

        public async Task DeleteManyAsync(Guid[] categoryIds, CancellationToken cancellationToken = default) 
            => await _dbContext.Categories.Where(c => categoryIds.Contains(c.Id)).ExecuteDeleteAsync(cancellationToken);

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default) 
            => await _dbContext.Categories.ToListAsync(cancellationToken);

        public async Task<Category?> GetAsync(Guid categoryId, CancellationToken cancellationToken = default) 
            => await _dbContext.Categories.SingleOrDefaultAsync(c => c.Id == categoryId, cancellationToken);

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
        
    }
}
