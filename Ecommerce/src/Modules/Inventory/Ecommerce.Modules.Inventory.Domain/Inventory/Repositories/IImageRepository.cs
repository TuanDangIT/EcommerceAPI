using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Repositories
{
    public interface IImageRepository
    {
        Task<IEnumerable<Guid>> GetAllImagesForProductAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Guid>> GetAllImagesForProductsAsync(Guid[] productIds, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<Image> images, CancellationToken cancellationToken = default);
    }
}
