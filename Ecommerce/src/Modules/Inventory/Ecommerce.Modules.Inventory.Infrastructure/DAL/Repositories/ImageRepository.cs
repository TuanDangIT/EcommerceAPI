using Ecommerce.Modules.Inventory.Domain.Repositories;
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
        public async Task<IEnumerable<Guid>> GetAllImagesForProductAsync(Guid productId)
            => await _dbContext.Images.Where(i => i.ProductId == productId).Select(i => i.Id).ToListAsync();
        public async Task<IEnumerable<Guid>> GetAllImagesForProductsAsync(Guid[] productIds)
            => await _dbContext.Images.Where(i => productIds.Contains(i.ProductId)).Select(i => i.Id).ToListAsync();
    }
}
