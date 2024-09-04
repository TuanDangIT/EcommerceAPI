﻿using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
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
        public async Task AddAsync(Category category)
        {
            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid categoryId) => await _dbContext.Categories.Where(c => c.Id == categoryId).ExecuteDeleteAsync();

        public async Task DeleteManyAsync(Guid[] categoryIds) => await _dbContext.Categories.Where(c => categoryIds.Contains(c.Id)).ExecuteDeleteAsync();

        public async Task<IEnumerable<Category>> GetAllAsync() => await _dbContext.Categories.ToListAsync();

        public async Task<Category?> GetAsync(Guid categoryId) => await _dbContext.Categories.SingleOrDefaultAsync(c => c.Id == categoryId);

        public async Task UpdateAsync()
            => await _dbContext.SaveChangesAsync();
        
    }
}
