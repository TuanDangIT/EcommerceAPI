using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Modules.Carts.Core.Mappings;
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

        public CartService(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProductAsync(Guid productId, Guid cartId)
        {
            var cart = await GetByCartIdWithIncludesOrThrowIfNull(cartId);
            var product = await GetProductOrThrowNull(productId);
            cart.AddProduct(product);
            var rowsChanged = await _dbContext.SaveChangesAsync();
            if(rowsChanged != 1)
            {
                throw new ProductNotAddedException();
            }
        }

        public async Task CheckoutAsync(Guid cartId)
        {
            var cart = await GetByCartIdWithIncludesOrThrowIfNull(cartId);
            var checkoutCart = cart.Checkout();
            await _dbContext.CheckoutCarts.AddAsync(checkoutCart);
            _dbContext.Carts.Remove(cart);
            var rowsChanged = await _dbContext.SaveChangesAsync();
            if(rowsChanged == 0)
            {
                throw new CartCheckoutFailedException();
            }
        }

        public async Task ClearCartAsync(Guid cartId)
        {
            var cart = await GetByCartIdOrThrowIfNull(cartId);
            cart.Clear();
            var rowsChanged = await _dbContext.SaveChangesAsync();
            if(rowsChanged != 1)
            {
                throw new CartNotClearedException();
            }
        }

        public async Task CreateAsync(Guid customerId)
        {
            await _dbContext.Carts.AddAsync(new Cart(customerId));
            var rowsChanged = await _dbContext.SaveChangesAsync();
            if(rowsChanged != 1)
            {
                throw new CartNotCreatedException();
            }
        }

        public async Task CreateAsync()
        {
            await _dbContext.Carts.AddAsync(new Cart());
            var rowsChanged = await _dbContext.SaveChangesAsync();
            if (rowsChanged != 1)
            {
                throw new CartNotCreatedException();
            }
        }

        public async Task<CartDto> GetAsync(Guid cartId)
        {
            var cart = await GetByCartIdWithIncludesOrThrowIfNull(cartId);
            return cart.AsDto();
        }

        public async Task RemoveProduct(Guid productId, Guid cartId)
        {
            var cart = await GetByCartIdWithIncludesOrThrowIfNull(cartId);
            var product = await GetProductOrThrowNull(productId);
            cart.RemoveProduct(product);
            var rowsChanged = await _dbContext.SaveChangesAsync();
            if(rowsChanged != 1)
            {
                throw new ProductNotRemovedException();
            }
        }
        private async Task<Cart> GetByCartIdOrThrowIfNull(Guid cartId)
        {
            var cart = await _dbContext.Carts
                .SingleOrDefaultAsync(c => c.Id == cartId);
            if(cart is null)
            {
                throw new CartNotFoundException(cartId);
            }
            return cart;
        }
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
        private async Task<Product> GetProductOrThrowNull(Guid productId)
        {
            var product = await _dbContext.Products
                .SingleOrDefaultAsync(p => p.Id == productId);
            if (product is null)
            {
                throw new ProductNotFoundException(productId);
            }
            return product;
        }
    }
}
