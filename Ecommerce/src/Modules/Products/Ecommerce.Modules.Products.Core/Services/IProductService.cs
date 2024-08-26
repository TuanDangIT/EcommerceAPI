using Ecommerce.Modules.Products.Core.DTO;
using Ecommerce.Modules.Products.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Services
{
    internal interface IProductService
    {
        //Task<int> AddAsync(Product product);
        //Task<int> UpdateAsync(Product product);
        //Task<int> DeleteAsync(Guid productId);
        //Task<int> DeleteManyAsync(Guid[] productIds);
        Task<IEnumerable<ProductBrowseDto>> GetAllAsync();
        Task<ProductDetailsDto?> GetAsync(Guid productId);
    }
}
