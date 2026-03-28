using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Repositories
{
    public interface IProductRepository
    {
        public Task AddAsync(Product product, CancellationToken cancellationToken = default);
        public Task AddAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default);
        public Task<Product?> GetAsync(Guid productId, CancellationToken cancellationToken = default);
        public Task<Product?> GetAsync(string sku, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Product>> GetAllThatContainsInArrayAsync(Guid[] productIds, CancellationToken cancellationToken = default);
        public Task DeleteAsync(Guid[] productId, CancellationToken cancellationToken = default);
        public Task UpdateAsync(CancellationToken cancellationToken = default);
    }
}
