using Coravel.Scheduling.Schedule.Interfaces;
using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DAL.Mappings;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Events;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Ecommerce.Modules.Carts.Tests.Unit")]
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

        public async Task AddProductAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);
            try
            {
                var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken,
                    query => query.Include(c => c.CheckoutCart).ThenInclude(cc => cc!.Discount),
                    query => query.Include(c => c.Products).ThenInclude(cp => cp.Product),
                    query => query.Include(c => c.Discount));
                //var product = await _dbContext.Products
                //    .FromSqlInterpolated($"SELECT * FROM carts.\"Products\" WHERE \"Id\" = {productId} FOR UPDATE NOWAIT")
                //    .SingleOrDefaultAsync(cancellationToken) ?? throw new ProductNotFoundException(productId);
                var product = await _dbContext.Products
                    .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken) 
                    ?? throw new ProductNotFoundException(productId);
                cart.AddProduct(product, quantity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Product: {productId} was added to cart: {cartId}.", product.Id, cart.Id);
                await _messageBroker.PublishAsync(new ProductReserved(productId, quantity));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<Guid> CheckoutAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken,
                query => query.Include(c => c.CheckoutCart),
                query => query.Include(c => c.Products),
                query => query.Include(c => c.Discount));
            var hasCheckout = cart.CheckoutCart is not null;
            var customerId = _contextService.Identity?.Id;
            var checkoutCart = cart.Checkout(customerId);
            if (!hasCheckout)
            {
                await _dbContext.CheckoutCarts.AddAsync(checkoutCart, cancellationToken);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Cart: {cartId} was checked out.", cart.Id);
            return checkoutCart.Id;
        }

        public async Task ClearAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken,
                query => query.Include(c => c.Products).ThenInclude(c => c.Product),
                query => query.Include(c => c.CheckoutCart));
            var products = cart.Products
                .GroupBy(cp => cp.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(cp => cp.Quantity));
            cart.Clear();
            //if(cart.CheckoutCart is not null)
            //{
            //    _dbContext.CheckoutCarts.Remove(cart.CheckoutCart);
            //}
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Cart: {cartId} was cleared.", cart.Id);
            await _messageBroker.PublishAsync(new ProductsUnreserved(products));
        }

        public async Task<Guid> CreateAsync(CancellationToken cancellationToken = default)
        {
            var entry = await _dbContext.Carts.AddAsync(new Cart(), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Cart: {cartId} was created.", entry.Entity.Id);
            return entry.Entity.Id;
        }

        public async Task<CartDto?> GetAsync(Guid cartId, CancellationToken cancellationToken = default)
            => await _dbContext.Carts
                .AsNoTracking()
                .Include(c => c.Products)
                .ThenInclude(cp => cp.Product)
                .Include(c => c.Discount)
                .Where(c => c.Id == cartId)
                .Select(c => c.AsDto())
                .FirstOrDefaultAsync(cancellationToken);

        public async Task RemoveProductAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken,
                query => query.Include(c => c.Products).ThenInclude(cp => cp.Product),
                query => query.Include(c => c.Discount),
                query => query.Include(c => c.CheckoutCart).ThenInclude(cc => cc!.Discount));
            var cartProduct = await _dbContext.CartProducts
                .Include(cp => cp.Product)
                .FirstOrDefaultAsync(cp => cp.Product.Id == productId && cp.Cart.Id == cartId, cancellationToken) ??
                throw new ProductNotFoundException(productId);
            var product = cartProduct.Product;
            cart.RemoveProduct(product, quantity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Product: {productId} was removed from cart: {cartId}.", product.Id, cart.Id);
            await _messageBroker.PublishAsync(new ProductUnreserved(productId, quantity));
        }
        public async Task SetProductQuantityAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken,
                query => query.Include(c => c.Products).ThenInclude(cp => cp.Product),
                query => query.Include(c => c.Discount), 
                query => query.Include(c => c.CheckoutCart).ThenInclude(cc => cc!.Discount));
            var product = cart.Products.FirstOrDefault(p => p.Product.Id == productId)?.Product ??
                throw new ProductNotFoundException(productId);
            var (isReservationRequired, diffrence) = cart.SetProductQuantity(product, quantity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Product quantity: {quantity} was set for product: {productId} in cart: {cartId}.", quantity, product.Id, cart.Id);
            switch (isReservationRequired)
            {
                case false:
                    await _messageBroker.PublishAsync(new ProductUnreserved(productId, diffrence));
                    break;
                case true:
                    await _messageBroker.PublishAsync(new ProductReserved(productId, diffrence));
                    break;
            }
        }
        public async Task AddDiscountAsync(Guid cartId, string code, CancellationToken cancellationToken = default)
        {
            var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken,
                query => query.Include(c => c.Discount),
                query => query.Include(c => c.Products).ThenInclude(cp => cp.Product),
                query => query.Include(c => c.CheckoutCart).ThenInclude(cc => cc!.Discount));
            var discount = await _dbContext.Discounts
                .FirstOrDefaultAsync(d => d.Code == code, cancellationToken) ??
                throw new DiscountNotFoundException(code);
            if (discount.HasCustomerId)
            {
                if (_contextService.Identity is null || _contextService.Identity.Id == Guid.Empty)
                {
                    throw new IndividualDiscountCannotBeAppliedException($"Cannot use individual discount: {discount.Id} without registering.");
                }
                if (discount.CustomerId != _contextService.Identity.Id ||
                    !cart.Products.Any(p => p.Product.SKU == discount.SKU))
                {
                    throw new IndividualDiscountCannotBeAppliedException($"Discount: {discount.Id} cannot be applied because of wrong user or SKU of a product.");
                }
            }
            cart.AddDiscount(discount);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Discount: {discountId} was redeemed for cart: {cartId}.", discount.Id, cart.Id);
        }
        public async Task RemoveDiscountAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken,
                query => query.Include(c => c.Discount),
                query => query.Include(c => c.Products).ThenInclude(cp => cp.Product),
                query => query.Include(c => c.CheckoutCart).ThenInclude(cc => cc!.Discount));
            if (!cart.HasDiscount)
            {
                throw new CheckoutCartCannotRemoveDiscountException();
            }
            var discountId = cart.Discount!.Id;
            cart.RemoveDiscount();
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Discount: {discountId} was removed from cart: {cartId}.", discountId, cart.Id);
        }
        private async Task<Cart> GetByCartOrThrowIfNullAsync(Guid cartId, CancellationToken cancellationToken = default,
            params Func<IQueryable<Cart>, IQueryable<Cart>>[] includeActions)
        {
            var query = _dbContext.Carts
                .AsQueryable();
            if (includeActions is not null)
            {
                foreach (var includeAction in includeActions)
                {
                    query = includeAction(query);
                }
            }
            var cart = await query
                .FirstOrDefaultAsync(c => c.Id == cartId, cancellationToken) ??
                throw new CartNotFoundException(cartId);
            return cart;
        }
    }
}
