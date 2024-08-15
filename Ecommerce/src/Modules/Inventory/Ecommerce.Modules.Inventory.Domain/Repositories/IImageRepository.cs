using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Repositories
{
    public interface IImageRepository
    {
        Task<IEnumerable<Guid>> GetAllImagesForProduct(Guid productId);
    }
}
