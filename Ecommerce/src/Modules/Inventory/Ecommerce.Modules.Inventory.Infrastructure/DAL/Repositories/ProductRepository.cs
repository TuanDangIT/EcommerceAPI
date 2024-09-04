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
    internal class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext _dbContext;

        public ProductRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid productId) => await _dbContext.Products.Where(p => p.Id == productId).ExecuteDeleteAsync();

        public async Task<IEnumerable<Guid>> GetAllIdThatContainsInArrayAsync(Guid[] productIds) 
            => await _dbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync();

        public Task<Product?> GetAsync(Guid productId) => _dbContext.Products.SingleOrDefaultAsync(p => p.Id == productId);

        public async Task<int> UpdateAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteProductParametersAndImagesRelatedToProduct(Guid productId)
        {
            await _dbContext.Images.Where(i => i.ProductId == productId).ExecuteDeleteAsync();
            await _dbContext.ProductParameters.Where(i => i.ProductId == productId).ExecuteDeleteAsync();
        }
        public async Task<int> DeleteManyAsync(params Guid[] productIds) => await _dbContext.Products.Where(p => productIds.Contains(p.Id)).ExecuteDeleteAsync();

        public async Task<IEnumerable<Product>> GetAllThatContainsInArrayAsync(Guid[] productIds)
            => await _dbContext.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .Include(p => p.Images.OrderBy(i => i.Order))
                .Include(p => p.ProductParameters)
                .ThenInclude(p => p.Parameter)
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
    }
}
