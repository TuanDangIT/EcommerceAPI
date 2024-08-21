using Ecommerce.Modules.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Repositories
{
    public interface IImageRepository
    {
        Task<IEnumerable<Guid>> GetAllImagesForProductAsync(Guid productId);
        Task<IEnumerable<Guid>> GetAllImagesForProductsAsync(Guid[] productIds);
        Task AddRangeAsync(IEnumerable<Image> images);
    }
}
