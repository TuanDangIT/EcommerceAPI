using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services
{
    public interface ICartService
    {
        Task<CartDto> GetAsync(Guid cartId);
        Task CreateAsync(Guid customerId);
        Task CreateAsync();
        Task AddProductAsync(Guid productId, Guid cartId);
        Task RemoveProduct(Guid productId, Guid cartId);
        Task ClearCartAsync(Guid cartId);
        Task CheckoutAsync(Guid cartId);
    }
}
