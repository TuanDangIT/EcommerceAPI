using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly OrdersDbContext _dbContext;

        public ProductRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default)
        {
            await _dbContext.Products.AddRangeAsync(products, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteAsync(Guid[] productId, CancellationToken cancellationToken = default)
            => _dbContext.Products.Where(p => productId.Contains(p.Id)).ExecuteDeleteAsync(cancellationToken);

        public async Task<IEnumerable<Product>> GetAllThatContainsInArrayAsync(Guid[] productIds, CancellationToken cancellationToken = default)
            => await _dbContext.Products.Where(p => productIds.Contains(p.Id)).ToListAsync(cancellationToken);

        public Task<Product?> GetAsync(Guid productId, CancellationToken cancellationToken = default)
            => _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

        public Task<Product?> GetAsync(string sku, CancellationToken cancellationToken = default)
            => _dbContext.Products.FirstOrDefaultAsync(p => p.SKU == sku, cancellationToken);

        public Task UpdateAsync(CancellationToken cancellationToken = default)
            => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
