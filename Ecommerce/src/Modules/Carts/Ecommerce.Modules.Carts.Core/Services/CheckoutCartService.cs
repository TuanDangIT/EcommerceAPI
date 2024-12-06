using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DAL.Mappings;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Entities.ValueObjects;
using Ecommerce.Modules.Carts.Core.Events;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Modules.Carts.Core.Services.Externals;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Infrastructure.Stripe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services
{
    internal class CheckoutCartService : ICheckoutCartService
    {
        private readonly ICartsDbContext _dbContext;
        private readonly IPaymentProcessorService _paymentProcessorService;
        private readonly IMessageBroker _messageBroker;
        private readonly IContextService _contextService;
        private readonly ILogger<CheckoutCartService> _logger;
        private readonly StripeOptions _stripeOptions;

        public CheckoutCartService(ICartsDbContext dbContext, IPaymentProcessorService paymentProcessorService, 
            IMessageBroker messageBroker, IContextService contextService, ILogger<CheckoutCartService> logger, StripeOptions stripeOptions)
        {
            _dbContext = dbContext;
            _paymentProcessorService = paymentProcessorService;
            _messageBroker = messageBroker;
            _contextService = contextService;
            _logger = logger;
            _stripeOptions = stripeOptions;
        }

        public async Task<CheckoutCartDto?> GetAsync(Guid checkoutCartId, CancellationToken cancellationToken = default)
            => await _dbContext.CheckoutCarts
                .Include(cc => cc.Payment)
                .Include(cc => cc.Products)
                .ThenInclude(cp => cp.Product)
                .Include(cc => cc.Discount)
                .AsNoTracking()
                .Where(cc => cc.Id == checkoutCartId)
                .Select(cc => cc.AsDto())
                .SingleOrDefaultAsync(cancellationToken);

        public async Task<CheckoutStripeSessionDto> PlaceOrderAsync(Guid checkoutCartId, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken,
                cc => cc.Payment, cc => cc.Discount);
            if(checkoutCart.Shipment is null || checkoutCart.Payment is null)
            {
                throw new CheckoutCartInvalidPlaceOrderException();
            }
            var dto = await _paymentProcessorService.CheckoutAsync(checkoutCart, cancellationToken);
            checkoutCart.SetStripeSessionId(dto.SessionId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Order was placed for checkout cart: {@checkoutCart}.", checkoutCart);
            return dto;
        }

        public async Task SetCustomerAsync(Guid checkoutCartId, CustomerDto customerDto, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken);
            checkoutCart.SetCustomer(new Entities.ValueObjects.Customer(
                customerDto.FirstName,
                customerDto.LastName,
                customerDto.Email,
                customerDto.PhoneNumber,
                _contextService.Identity?.Id
                ));
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Set customer: {@customer} for checkout cart: {@checkoutCart}.", customerDto, checkoutCart);
        }
        public async Task SetPaymentAsync(Guid checkoutCartId, Guid paymentId, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken, cc => cc.Payment);
            var payment = await _dbContext.Payments
                .SingleOrDefaultAsync(p => p.Id == paymentId, cancellationToken) ?? 
                throw new PaymentNotFoundException(paymentId);
            checkoutCart.SetPayment(payment);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Set payment: {@payment} for checkout cart: {@checkoutCart}.", payment, checkoutCart);
        }

        public async Task SetShipmentAsync(Guid checkoutCartId, ShipmentDto shipmentDto, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken);
            checkoutCart.SetShipment(new Shipment(
                shipmentDto.Country,
                shipmentDto.City,
                shipmentDto.PostalCode,
                shipmentDto.StreetName,
                shipmentDto.StreetNumber,
                shipmentDto.AparmentNumber
                ));
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Set shipment: {@shipment} for checkout cart: {@checkoutCart}.", shipmentDto, checkoutCart);
        }
        public async Task SetAdditionalInformationAsync(Guid checkoutCartId, string additionalInformation, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken);
            checkoutCart.SetAdditionalInformation(additionalInformation);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Set additional information: {@additionalInformation} for checkout cart: {@checkoutCart}.", additionalInformation, checkoutCart);
        }
        public async Task SetCheckoutCartDetailsAsync(Guid checkoutCartId, CheckoutCartSetDetailsDto checkoutCartSetDetailsDto, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken);
            var shipmentDto = checkoutCartSetDetailsDto.ShipmentDto;
            var customerDto = checkoutCartSetDetailsDto.CustomerDto;
            var paymentId = checkoutCartSetDetailsDto.PaymentId;
            var additionalInformation = checkoutCartSetDetailsDto.AdditionalInformation;
            checkoutCart.SetCustomer(new Entities.ValueObjects.Customer(
                customerDto.FirstName,
                customerDto.LastName,
                customerDto.Email,
                customerDto.PhoneNumber,
                _contextService.Identity?.Id
                ));
            checkoutCart.SetShipment(new Shipment(
                shipmentDto.Country,
                shipmentDto.City,
                shipmentDto.PostalCode,
                shipmentDto.StreetName,
                shipmentDto.StreetNumber,
                shipmentDto.AparmentNumber
                ));
            var payment = await _dbContext.Payments
                .SingleOrDefaultAsync(p => p.Id == paymentId, cancellationToken) ?? throw new PaymentNotFoundException(paymentId);
            checkoutCart.SetPayment(payment);
            if(additionalInformation is not null)
            {
                checkoutCart.SetAdditionalInformation(additionalInformation);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Set needed details {@checkoutCartDetails} for checkout cart: {@checkoutCart}.", checkoutCartSetDetailsDto, checkoutCart);
        }
        public async Task AddDiscountAsync(Guid checkoutCartId, string code, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken);
            var discount = await _dbContext.Discounts
                .SingleOrDefaultAsync(d => d.Code == code, cancellationToken) ?? 
                throw new DiscountNotFoundException(code);
            if (discount.CustomerId is not null && _contextService.Identity is null)
            {
                throw new IndividualDiscountCannotBeAppliedException("Cannot use individual discount without registering");
            }
            if (discount.CustomerId is not null && discount.CustomerId != _contextService.Identity!.Id &&
                !checkoutCart.Products.Select(p => p.Product.SKU).Contains(discount.SKU))
            {
                throw new IndividualDiscountCannotBeAppliedException("Discount cannot be applied because of wrong user or SKU of a product.");
            }
            checkoutCart.AddDiscount(discount);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Discount: {@discount} redeemed for checkout cart: {@checkoutCart}.", discount, checkoutCart);
        }
        public async Task HandleCheckoutSessionCompletedAsync(string? json, string? stripeSignatureHeader)
        {
            var stripeEvent = EventUtility.ConstructEvent(json, stripeSignatureHeader, _stripeOptions.WebhookSecret);
            Session? session = null;
            if (stripeEvent.Type != Stripe.Events.CheckoutSessionCompleted)
            {
                return;
            }
            session = stripeEvent.Data.Object as Session;
            if (session is null)
            {
                throw new NullReferenceException();
            }
            var sessionId = session.Id;
            var checkoutCart = await _dbContext.CheckoutCarts
                .Include(cc => cc.Products)
                .ThenInclude(cp => cp.Product)
                .Include(cc => cc.Payment)
                .Include(cc => cc.Discount)
                .SingleOrDefaultAsync(cc => cc.StripeSessionId == sessionId) ?? throw new CheckoutCartNotFoundException(sessionId);
            checkoutCart.SetStripePaymentIntentId(session.PaymentIntentId);
            checkoutCart.SetPaid();
            await _messageBroker.PublishAsync(new CustomerPlacedOrder()
            {
                CustomerId = checkoutCart.Customer.CustomerId,
                FirstName = checkoutCart.Customer.FirstName,
                LastName = checkoutCart.Customer.LastName,
                Email = checkoutCart.Customer.Email,
                PhoneNumber = checkoutCart.Customer.PhoneNumber,
                Products = checkoutCart.Products.Select(cp => new
                {
                    cp.Product.Id,
                    cp.Product.SKU,
                    cp.Product.Name,
                    cp.Product.Price,
                    cp.Quantity,
                    cp.Product.ImagePathUrl
                }),
                TotalSum = checkoutCart.TotalSum(),
                Country =   checkoutCart.Shipment!.Country,
                City = checkoutCart.Shipment!.City,
                PostalCode = checkoutCart.Shipment.PostalCode,
                StreetName = checkoutCart.Shipment.StreetName,
                StreetNumber = checkoutCart.Shipment.StreetNumber,
                ApartmentNumber = checkoutCart.Shipment.AparmentNumber,
                PaymentMethod = checkoutCart.Payment!.PaymentMethod.ToString(),
                AdditionalInformation = checkoutCart.AdditionalInformation,
                DiscountCode = checkoutCart.Discount?.Code,
                StripePaymentIntentId = checkoutCart.StripePaymentIntentId!

            });
            if(checkoutCart.Discount is not null)
            {
                await _messageBroker.PublishAsync(new DiscountCodeRedeemed(checkoutCart.Discount.Code));
            }
            _dbContext.CartProducts.RemoveRange(checkoutCart.Products);
            _dbContext.CheckoutCarts.Remove(checkoutCart);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Handling session checkout completed for {@checkoutCart}.", checkoutCart);
        }
        private async Task<CheckoutCart> GetCheckoutCartOrThrowIfNullAsync(Guid checkoutCartId, CancellationToken cancellationToken = default, 
            params Expression<Func<CheckoutCart, object?>>[] includes)
        {
            var checkoutCarts = _dbContext.CheckoutCarts
                .Include(cc => cc.Products)
                .ThenInclude(cp => cp.Product)
                .AsQueryable();
            if(includes is not null)
            {
                foreach (var include in includes)
                {
                    checkoutCarts = checkoutCarts.Include(include);
                }
            }
            var checkoutCart = await checkoutCarts
                .SingleOrDefaultAsync(cc => cc.Id == checkoutCartId, cancellationToken) ?? 
                throw new CheckoutCartNotFoundException(checkoutCartId);
            return checkoutCart;
        }
    }
}
