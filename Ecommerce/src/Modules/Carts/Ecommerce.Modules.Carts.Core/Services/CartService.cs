using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DAL.Mappings;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Events;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartsDbContext dbContext, IContextService contextService, IMessageBroker messageBroker, ILogger<CartService> logger)
        {
            _dbContext = dbContext;
            _contextService = contextService;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task AddProductAsync(Guid cartId, Guid productId, int quantity)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var cart = await GetByCartOrThrowIfNull(cartId);
                var product = await _dbContext.Products
                    .FromSqlInterpolated($"SELECT * FROM carts.\"Products\" WHERE \"Id\" = {productId} FOR UPDATE NOWAIT")
                    .SingleOrDefaultAsync();
                if (product is null)
                {
                    _logger.LogError("Product: {productId} was not found.", productId);
                    throw new ProductNotFoundException(productId);
                }
                cart.AddProduct(product, quantity);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                await _messageBroker.PublishAsync(new ProductReserved(productId, quantity));
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.LogError("An exception occured during the transaction: {message}", e.Message);
                throw;
            }
        }

        public async Task CheckoutAsync(Guid cartId)
        {
            var cart = await GetByCartOrThrowIfNull(cartId);
            var checkoutCart = await _dbContext.CheckoutCarts.SingleOrDefaultAsync(cc => cc.Id == cartId);
            if(checkoutCart is not null)
            {
                return;
            }
            checkoutCart = cart.Checkout();
            await _dbContext.CheckoutCarts.AddAsync(checkoutCart);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearAsync(Guid cartId)
        {
            var cart = await GetByCartOrThrowIfNull(cartId);
            IEnumerable<object> products = cart.Products.Select(cp => new { cp.Product.Id, cp.Quantity }).ToList();
            await _messageBroker.PublishAsync(new CartCleared(products));
            cart.Clear();
            await _dbContext.SaveChangesAsync();
        }

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

        public async Task<CartDto?> GetAsync(Guid cartId)
        {
            var cart = await _dbContext.Carts
               .Include(c => c.Products)
               .ThenInclude(cp => cp.Product)
               .SingleOrDefaultAsync(c => c.Id == cartId);
            return cart?.AsDto();
        }

        public async Task RemoveProduct(Guid cartId, Guid productId, int quantity)
        {
            var cart = await GetByCartOrThrowIfNull(cartId);
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var cartProduct = await _dbContext.CartProducts
                    .FromSqlInterpolated(
                        $"SELECT cp.\"Id\", cp.\"CartId\", cp.\"CheckoutCartId\", cp.\"ProductId\", p.\"Id\", p.\"Quantity\" FROM carts.\"CartProducts\" cp JOIN carts.\"Products\" p ON p.\"Id\" = cp.\"ProductId\" JOIN carts.\"Carts\" c ON c.\"Id\" = cp.\"CartId\" WHERE p.\"Id\" = {productId} AND c.\"Id\" = {cartId} FOR UPDATE NOWAIT")
                    .SingleOrDefaultAsync();
                if (cartProduct is null)
                {
                    throw new ProductNotFoundException(productId);
                }
                var product = cartProduct.Product;
                cart.RemoveProduct(product, quantity);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                await _messageBroker.PublishAsync(new ProductUnreserved(productId, quantity));
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                _logger.LogError("An exception occured during the transaction: {message}", e.Message);
                throw;
            }
        }
        public async Task SetProductQuantity(Guid cartId, Guid productId, int quantity)
        {
            var cart = await GetByCartOrThrowIfNull(cartId);
            var product = await GetCartProductOrThrowIfNull(cartId, productId);
            cart.SetProductQuantity(product, quantity);
            await _dbContext.SaveChangesAsync();
        }
        private async Task<Cart> GetByCartOrThrowIfNull(Guid cartId)
        {
            var cart = await _dbContext.Carts
               .Include(c => c.Products)
               .ThenInclude(cp => cp.Product)
               .SingleOrDefaultAsync(c => c.Id == cartId);
            if (cart is null)
            {
                _logger.LogError("Cart: {cartId} was not found.", cartId);
                throw new CartNotFoundException(cartId);
            }
            return cart;
        }
        private async Task<Product> GetCartProductOrThrowIfNull(Guid cartId, Guid productId)
        {
            var cartProduct = await _dbContext.CartProducts
                .Include(cp => cp.Product)
                .Include(cp => cp.Cart)
                .SingleOrDefaultAsync(cp => cp.Product.Id == productId && cp.Cart.Id == cartId);
            if (cartProduct is null)
            {
                _logger.LogError("Product: {productId} was not found", productId);
                throw new ProductNotFoundException(productId);
            }
            return cartProduct.Product;
        }
    }
}
