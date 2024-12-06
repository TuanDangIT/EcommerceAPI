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
        Task<CartDto?> GetAsync(Guid cartId, CancellationToken cancellationToken = default);
        Task<Guid> CreateAsync(CancellationToken cancellationToken = default);
        Task AddProductAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default);
        Task RemoveProductAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default);
        Task SetProductQuantityAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default);
        Task ClearAsync(Guid cartId, CancellationToken cancellationToken = default);
        Task CheckoutAsync(Guid cartId, CancellationToken cancellationToken = default);
    }
}
