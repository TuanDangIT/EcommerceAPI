using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task AddManyAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddRangeAsync(products, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);    
        }

        public async Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default) 
            => await _dbContext.Products.Where(p => p.Id == productId).ExecuteDeleteAsync(cancellationToken);

        public Task<Product?> GetAsync(Guid productId, CancellationToken cancellationToken = default) 
            => _dbContext.Products.SingleOrDefaultAsync(p => p.Id == productId, cancellationToken);

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
        
        public async Task DeleteProductParametersAndImagesRelatedToProduct(Guid productId, CancellationToken cancellationToken = default)
        {
            await _dbContext.Images.Where(i => i.ProductId == productId).ExecuteDeleteAsync(cancellationToken);
            await _dbContext.ProductParameters.Where(i => i.ProductId == productId).ExecuteDeleteAsync();
        }
        public async Task DeleteManyAsync(CancellationToken cancellationToken = default, params Guid[] productIds) 
            => await _dbContext.Products.Where(p => productIds.Contains(p.Id)).ExecuteDeleteAsync(cancellationToken);

        public async Task<IEnumerable<Product>> GetAllThatContainsInArrayAsync(Guid[] productIds, CancellationToken cancellationToken = default,
            params Func<IQueryable<Product>, IQueryable<Product>>[] includeActions)
        {
            var query = _dbContext.Products
                .AsQueryable();
            if (includeActions is not null)
            {
                foreach (var includeAction in includeActions)
                {
                    query = includeAction(query);
                }
            }
            var products = await query
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync(cancellationToken);
            return products;
        }
        public async Task UpdateListedFlagAsync(Guid[] productIds, bool isListed, CancellationToken cancellationToken = default)
            => await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ExecuteUpdateAsync(p => p.SetProperty(p => p.IsListed, isListed), cancellationToken);

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
}
