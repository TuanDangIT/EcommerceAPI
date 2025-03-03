using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        Task AddManyAsync(IEnumerable<Product> products, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(CancellationToken cancellationToken = default, params Guid[] productIds);
        Task<IEnumerable<Guid>> GetAllIdThatContainsInArrayAsync(Guid[] productIds, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetAllThatContainsInArrayAsync(Guid[] productIds, CancellationToken cancellationToken = default,
            params Func<IQueryable<Product>, IQueryable<Product>>[] includeActions);
        Task<Product?> GetAsync(Guid productId, CancellationToken cancellationToken = default);
        Task DeleteProductParametersAndImagesRelatedToProduct(Guid productId, CancellationToken cancellationToken = default);
        Task UpdateListedFlagAsync(Guid[] productIds, bool isListed, CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
