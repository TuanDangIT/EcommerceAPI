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
    internal class ImageRepository : IImageRepository
    {
        private readonly InventoryDbContext _dbContext;

        public ImageRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Guid>> GetAllImagesForProductAsync(Guid productId, CancellationToken cancellationToken = default)
            => await _dbContext.Images.Where(i => i.ProductId == productId).Select(i => i.Id).ToListAsync(cancellationToken);
        public async Task<IEnumerable<Guid>> GetAllImagesForProductsAsync(Guid[] productIds, CancellationToken cancellationToken = default)
            => await _dbContext.Images.Where(i => productIds.Contains(i.ProductId)).Select(i => i.Id).ToListAsync(cancellationToken);
        public async Task AddRangeAsync(IEnumerable<Image> images, CancellationToken cancellationToken = default)
        {
            await _dbContext.Images.AddRangeAsync(images, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
