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
        Task<CartDto?> GetAsync(Guid cartId);
        //Task<Guid> CreateAsync(Guid customerId);
        Task<Guid> CreateAsync();
        Task AddProductAsync(Guid cartId, Guid productId, int quantity);
        Task RemoveProduct(Guid cartId, Guid productId, int quantity);
        Task SetProductQuantity(Guid cartId, Guid productId, int quantity);
        Task ClearCartAsync(Guid cartId);
        Task CheckoutAsync(Guid cartId);
        Task ResetCartAsync(Guid cartId);
    }
}
