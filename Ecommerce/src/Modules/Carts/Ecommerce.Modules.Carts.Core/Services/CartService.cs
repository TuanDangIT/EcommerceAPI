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
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken);
                var product = await _dbContext.Products
                    .FromSqlInterpolated($"SELECT * FROM carts.\"Products\" WHERE \"Id\" = {productId} FOR UPDATE NOWAIT")
                    .SingleOrDefaultAsync(cancellationToken) ?? throw new ProductNotFoundException(productId);
                cart.AddProduct(product, quantity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Product: {@product} was added to cart: {@cart}.", product, cart);
                await _messageBroker.PublishAsync(new ProductReserved(productId, quantity));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task CheckoutAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken);
            var checkoutCart = await _dbContext.CheckoutCarts
                .SingleOrDefaultAsync(cc => cc.Id == cartId, cancellationToken);
            if(checkoutCart is not null)
            {
                _logger.LogInformation("Cart: {@cart} was already checked out.", cart);   
                return;
            }
            checkoutCart = cart.Checkout();
            await _dbContext.CheckoutCarts.AddAsync(checkoutCart, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Cart: {@cart} was checked out.", cart);
        }

        public async Task ClearAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken);
            IEnumerable<object> products = cart.Products
                .Select(cp => new { cp.Product.Id, cp.Quantity })
                .ToList();
            cart.Clear();
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Cart: {@cart} was cleared.", cart);
            await _messageBroker.PublishAsync(new CartCleared(products));
        }

        public async Task<Guid> CreateAsync(CancellationToken cancellationToken = default)
        {
            var userId = _contextService.Identity?.Id;
            EntityEntry<Cart> entry;
            entry = await _dbContext.Carts.AddAsync(new Cart(userId), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Cart was created.");
            return entry.Entity.Id;
        }

        public async Task<CartDto?> GetAsync(Guid cartId, CancellationToken cancellationToken = default)
            => await _dbContext.Carts
                .AsNoTracking()
                .Include(c => c.Products)
                .ThenInclude(cp => cp.Product)
                .Where(c => c.Id == cartId)
                .Select(c => c.AsDto())
                .SingleOrDefaultAsync(cancellationToken);

        public async Task RemoveProductAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken);
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var cartProduct = await _dbContext.CartProducts
                    .FromSqlInterpolated(
                        $"SELECT cp.\"Id\", cp.\"CartId\", cp.\"CheckoutCartId\", cp.\"ProductId\", p.\"Id\", p.\"Quantity\" FROM carts.\"CartProducts\" cp JOIN carts.\"Products\" p ON p.\"Id\" = cp.\"ProductId\" JOIN carts.\"Carts\" c ON c.\"Id\" = cp.\"CartId\" WHERE p.\"Id\" = {productId} AND c.\"Id\" = {cartId} FOR UPDATE NOWAIT")
                    .SingleOrDefaultAsync(cancellationToken) ?? 
                    throw new ProductNotFoundException(productId);
                var product = cartProduct.Product;
                cart.RemoveProduct(product, quantity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation("Product: {@product} was removed from cart: {@cart}.", product, cart);
                await _messageBroker.PublishAsync(new ProductUnreserved(productId, quantity));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        public async Task SetProductQuantityAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default)
        {
            var cart = await GetByCartOrThrowIfNullAsync(cartId, cancellationToken);
            var product = cart.Products.SingleOrDefault(p => p.Product.Id == productId)?.Product ??
                throw new ProductNotFoundException(productId);
            cart.SetProductQuantity(product, quantity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Set product quantity: {quantity} for product: {@product} in cart: {@cart}.", quantity, product, cart);
        }
        private async Task<Cart> GetByCartOrThrowIfNullAsync(Guid cartId, CancellationToken cancellationToken = default)
            => await _dbContext.Carts
                .Include(c => c.Products)
                .ThenInclude(cp => cp.Product)
                .SingleOrDefaultAsync(c => c.Id == cartId, cancellationToken) ??
                throw new CartNotFoundException(cartId);
    }
}
