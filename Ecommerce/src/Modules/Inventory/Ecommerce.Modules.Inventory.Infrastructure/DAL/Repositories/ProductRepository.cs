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

        public async Task<IEnumerable<Guid>> GetAllIdThatContainsInArrayAsync(Guid[] productIds, CancellationToken cancellationToken = default) 
            => await _dbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

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

        public async Task<IEnumerable<Product>> GetAllThatContainsInArrayAsync(Guid[] productIds, CancellationToken cancellationToken = default)
            => await _dbContext.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.Category)
                .Include(p => p.Images.OrderBy(i => i.Order))
                .Include(p => p.ProductParameters)
                .ThenInclude(p => p.Parameter)
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync(cancellationToken);
        public async Task UpdateListedFlagAsync(Guid[] productIds, bool isListed, CancellationToken cancellationToken = default)
            => await _dbContext.Products
                .Where(p => productIds.Contains(p.Id))
                .ExecuteUpdateAsync(p => p.SetProperty(p => p.IsListed, isListed), cancellationToken);

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
}
