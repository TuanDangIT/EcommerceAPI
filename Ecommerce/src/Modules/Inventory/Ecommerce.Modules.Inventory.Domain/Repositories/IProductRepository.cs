using Ecommerce.Modules.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<int> AddAsync(Product product);
        Task<int> UpdateAsync(Product product);
        Task<int> DeleteAsync(Guid productId);
        //Task<int> DecreaseQuantityAsync(Guid productId, int ammount);
        Task<int> DeleteManyAsync(Guid[] productIds);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetAsync(Guid productId);
    }
}
