using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Modules.Carts.Core.Mappings;
using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services
{
    internal class CartService : ICartService
    {
        private readonly ICartsDbContext _dbContext;
        private readonly IContextService _contextService;

        public CartService(ICartsDbContext dbContext, IContextService contextService)
        {
            _dbContext = dbContext;
            _contextService = contextService;
        }

        public async Task AddProductAsync(Guid cartId, Guid productId, int quantity)
        {
            var cart = await GetByCartIdWithIncludesOrThrowIfNull(cartId);
            var product = await _dbContext.Products
                .SingleOrDefaultAsync(p => p.Id == productId);
            if (product is null)
            {
                throw new ProductNotFoundException(productId);
            }
            cart.AddProduct(product, quantity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CheckoutAsync(Guid cartId)
        {
            var cart = await GetByCartIdWithIncludesOrThrowIfNull(cartId);
            var checkoutCart = cart.Checkout();
            await _dbContext.CheckoutCarts.AddAsync(checkoutCart);
            _dbContext.Carts.Remove(cart);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Guid cartId)
        {
            var cart = await GetByCartIdWithIncludesOrThrowIfNull(cartId);
            cart.Clear();
            await _dbContext.SaveChangesAsync();
        }

        //public async Task<Guid> CreateAsync(Guid customerId)
        //{
        //    var newGuid = Guid.NewGuid();
        //    await _dbContext.Carts.AddAsync(new Cart(newGuid, customerId));
        //    await _dbContext.SaveChangesAsync();
        //    return newGuid;
        //}

        public async Task<Guid> CreateAsync()
        {
            var newGuid = Guid.NewGuid();
            var userId = _contextService.Identity is not null &&
                _contextService.Identity.IsAuthenticated ?
                _contextService.Identity.Id :
                Guid.Empty;
            if(userId == Guid.Empty)
            {
                await _dbContext.Carts.AddAsync(new Cart(newGuid));
            }
            else
            {
                await _dbContext.Carts.AddAsync(new Cart(newGuid, userId));
            }
            await _dbContext.SaveChangesAsync();
            return newGuid;
        }

        public async Task<CartDto> GetAsync(Guid cartId)
        {
            var cart = await GetByCartIdWithIncludesOrThrowIfNull(cartId);
            return cart.AsDto();
        }

        public async Task RemoveProduct(Guid cartId, Guid productId)
        {
            var cart = await GetByCartIdWithIncludesOrThrowIfNull(cartId);
            var product = await GetCartProductOrThrowNull(cartId, productId);
            cart.RemoveProduct(product);
            await _dbContext.SaveChangesAsync();
        }
        public async Task SetProductQuantity(Guid cartId, Guid productId, int quantity)
        {
            var cart = await GetByCartIdWithIncludesOrThrowIfNull(cartId);
            var product = await GetCartProductOrThrowNull(cartId, productId);
            cart.SetProductQuantity(product, quantity);
            await _dbContext.SaveChangesAsync();
        }
        //private async Task<Cart> GetByCartIdOrThrowIfNull(Guid cartId)
        //{
        //    var cart = await _dbContext.Carts
        //        .SingleOrDefaultAsync(c => c.Id == cartId);
        //    if(cart is null)
        //    {
        //        throw new CartNotFoundException(cartId);
        //    }
        //    return cart;
        //}
        private async Task<Cart> GetByCartIdWithIncludesOrThrowIfNull(Guid cartId)
        {
            var cart = await _dbContext.Carts
               .Include(c => c.Products)
               .ThenInclude(cp => cp.Product)
               .SingleOrDefaultAsync(c => c.Id == cartId);
            if (cart is null)
            {
                throw new CartNotFoundException(cartId);
            }
            return cart;
        }
        private async Task<Product> GetCartProductOrThrowNull(Guid cartId, Guid productId)
        {
            var cartProduct = await _dbContext.CartProducts
                .Include(cp => cp.Product)
                .Include(cp => cp.Cart)
                .SingleOrDefaultAsync(cp => cp.Product.Id == productId && cp.Cart.Id == cartId);
            if (cartProduct is null)
            {
                throw new ProductNotFoundException(productId);
            }
            return cartProduct.Product;
        }
    }
}
