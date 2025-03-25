using Coravel.Scheduling.Schedule.Interfaces;
using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DAL.Mappings;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Entities.ValueObjects;
using Ecommerce.Modules.Carts.Core.Events;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Modules.Carts.Core.Scheduler;
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
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services
{
    internal class CheckoutCartService : ICheckoutCartService
    {
        private readonly ICartsDbContext _dbContext;
        private readonly IPaymentProcessorService _paymentProcessorService;
        private readonly TimeProvider _timeProvider;
        private readonly IMessageBroker _messageBroker;
        private readonly IContextService _contextService;
        private readonly ILogger<CheckoutCartService> _logger;
        private readonly StripeOptions _stripeOptions;
        private readonly IScheduler _scheduler;

        public CheckoutCartService(ICartsDbContext dbContext, IPaymentProcessorService paymentProcessorService, TimeProvider timeProvider,
            IMessageBroker messageBroker, IContextService contextService, ILogger<CheckoutCartService> logger, StripeOptions stripeOptions, IScheduler scheduler)
        {
            _dbContext = dbContext;
            _paymentProcessorService = paymentProcessorService;
            _timeProvider = timeProvider;
            _messageBroker = messageBroker;
            _contextService = contextService;
            _logger = logger;
            _stripeOptions = stripeOptions;
            _scheduler = scheduler;
        }

        public async Task<CheckoutCartDto?> GetAsync(Guid checkoutCartId, CancellationToken cancellationToken = default)
            => await _dbContext.CheckoutCarts
                .Include(cc => cc.Payment)
                .Include(cc => cc.Products)
                .ThenInclude(cp => cp.Product)
                .Include(cc => cc.Discount)
                .AsNoTracking()
                .Where(cc => cc.Id == checkoutCartId && cc.IsPaid == false)
                .Select(cc => cc.AsDto())
                .FirstOrDefaultAsync(cancellationToken);

        public async Task<CheckoutStripeSessionDto> PlaceOrderAsync(Guid checkoutCartId, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken,
                cc => cc.Payment, cc => cc.Discount);
            if (checkoutCart.Discount?.ExpiresAt is not null && checkoutCart.Discount?.ExpiresAt < _timeProvider.GetUtcNow().UtcDateTime)
            {
                throw new DiscountExpiredException();
            }
            ValidateCartIsNotPaid(checkoutCart);
            if (checkoutCart.Shipment is null || checkoutCart.Payment is null)
            {
                throw new CheckoutCartInvalidPlaceOrderException();
            }
            var dto = await _paymentProcessorService.CheckoutAsync(checkoutCart, cancellationToken);
            checkoutCart.SetStripeSessionId(dto.SessionId);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Order was placed for checkout cart: {checkoutCartId}.", checkoutCart.Id);
            return dto;
        }

        public async Task SetCustomerAsync(Guid checkoutCartId, CustomerDto customerDto, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken);
            ValidateCartIsNotPaid(checkoutCart);
            checkoutCart.SetCustomer(new Entities.ValueObjects.Customer(
                customerDto.FirstName,
                customerDto.LastName,
                customerDto.Email,
                customerDto.PhoneNumber,
                _contextService.Identity?.Id
                ));
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Customer: {@customer} was set for checkout cart: {checkoutCartId}.", customerDto, checkoutCart.Id);
        }

        public async Task SetPaymentAsync(Guid checkoutCartId, Guid paymentId, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken, cc => cc.Payment);
            ValidateCartIsNotPaid(checkoutCart);
            var payment = await _dbContext.Payments
                .FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken) ?? 
                throw new PaymentNotFoundException(paymentId);
            checkoutCart.SetPayment(payment);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Payment: {paymentId} was set for checkout cart: {checkoutCartId}.", payment.Id, checkoutCart.Id);
        }

        public async Task FillShipmentDetailsAsync(Guid checkoutCartId, ShipmentFillDto shipmentFillDto, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken);
            ValidateCartIsNotPaid(checkoutCart);
            checkoutCart.FillShipment(new Shipment(
                shipmentFillDto.Country,
                shipmentFillDto.City,
                shipmentFillDto.PostalCode,
                shipmentFillDto.StreetName,
                shipmentFillDto.StreetNumber,
                shipmentFillDto.AparmentNumber
                ));
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Shipment: {@shipment} was set for checkout cart: {checkoutCartId}.", shipmentFillDto, checkoutCart.Id);
        }

        public async Task SetAdditionalInformationAsync(Guid checkoutCartId, string additionalInformation, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken);
            ValidateCartIsNotPaid(checkoutCart);
            checkoutCart.SetAdditionalInformation(additionalInformation);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Additional information: {additionalInformation} was set for checkout cart: {checkoutCartId}.", additionalInformation, checkoutCart.Id);
        }

        public async Task SetCustomerIdAsync(Guid checkoutCartId, Guid? customerId, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await _dbContext.CheckoutCarts
                .FirstOrDefaultAsync(cc => cc.Id == checkoutCartId, cancellationToken) ?? throw new CheckoutCartNotFoundException(checkoutCartId);
            ValidateCartIsNotPaid(checkoutCart);
            var customerIdToSet = (customerId ?? _contextService.Identity?.Id) ?? throw new CustomerIdNullException();
            checkoutCart.SetCustomerId((Guid)customerIdToSet);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Customer id: {customerId} was set for checkout cart: {checkoutCartId}", customerIdToSet, checkoutCart.Id);
        }

        public async Task SetCheckoutCartDetailsAsync(Guid checkoutCartId, CheckoutCartSetDetailsDto checkoutCartSetDetailsDto, CancellationToken cancellationToken = default)
        {
            var checkoutCart = await GetCheckoutCartOrThrowIfNullAsync(checkoutCartId, cancellationToken);
            ValidateCartIsNotPaid(checkoutCart);
            var shipmentFillDto = checkoutCartSetDetailsDto.ShipmentFillDto;
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
            checkoutCart.FillShipment(new Shipment(
                shipmentFillDto.Country,
                shipmentFillDto.City,
                shipmentFillDto.PostalCode,
                shipmentFillDto.StreetName,
                shipmentFillDto.StreetNumber,
                shipmentFillDto.AparmentNumber
                ));
            var payment = await _dbContext.Payments
                .FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken) ?? throw new PaymentNotFoundException(paymentId);
            checkoutCart.SetPayment(payment);
            if(additionalInformation is not null)
            {
                checkoutCart.SetAdditionalInformation(additionalInformation);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Necessary details {@checkoutCartDetails} were set for checkout cart: {checkoutCartId}.", checkoutCartSetDetailsDto, checkoutCart.Id);
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
                .Include(cc => cc.Cart)
                .FirstOrDefaultAsync(cc => cc.StripeSessionId == sessionId) ?? throw new CheckoutCartNotFoundException(sessionId);
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
                    Price = cp.DiscountedPrice ?? cp.Price,
                    UnitPrice = cp.DiscountedPrice is null ? cp.Product.Price : cp.DiscountedPrice/cp.Quantity,
                    cp.Quantity,
                    cp.Product.ImagePathUrl
                }),
                TotalSum = checkoutCart.TotalSum,
                ShippingService = checkoutCart.Shipment!.Service,
                ShippingPrice = checkoutCart.Shipment!.Price,
                Country =   checkoutCart.Shipment!.Country,
                City = checkoutCart.Shipment!.City,
                PostalCode = checkoutCart.Shipment.PostalCode,
                StreetName = checkoutCart.Shipment.StreetName,
                StreetNumber = checkoutCart.Shipment.StreetNumber,
                ApartmentNumber = checkoutCart.Shipment.AparmentNumber,
                PaymentMethod = checkoutCart.Payment!.PaymentMethod.ToString(),
                AdditionalInformation = checkoutCart.AdditionalInformation,
                DiscountCode = checkoutCart.Discount?.Code,
                DiscountType = checkoutCart.Discount?.Type.ToString(),
                DiscountValue = checkoutCart.Discount?.Value,
                DiscountSku = checkoutCart.Discount?.SKU,
                StripePaymentIntentId = checkoutCart.StripePaymentIntentId!
            });
            if(checkoutCart.Discount is not null)
            {
                await _messageBroker.PublishAsync(new DiscountCodeRedeemed(checkoutCart.Discount.Code));
            }
            //_dbContext.Carts.Remove(checkoutCart.Cart);
            _scheduler.ScheduleWithParams<DeleteCartAfterSuccessfulPaymentTask>(checkoutCart.Cart.Id)
                .EveryFiveSeconds()
                .Once();
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Handling session checkout was completed for {checkoutCartId}.", checkoutCart.Id);
        }

        private async Task<CheckoutCart> GetCheckoutCartOrThrowIfNullAsync(Guid checkoutCartId, CancellationToken cancellationToken = default, 
            params Expression<Func<CheckoutCart, object?>>[] includes)
        {
            var query = _dbContext.CheckoutCarts
                .Include(cc => cc.Products)
                .ThenInclude(cp => cp.Product)
                .AsQueryable();
            if(includes is not null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            var checkoutCart = await query
                .FirstOrDefaultAsync(cc => cc.Id == checkoutCartId, cancellationToken) ?? 
                throw new CheckoutCartNotFoundException(checkoutCartId);
            return checkoutCart;
        }

        private void ValidateCartIsNotPaid(CheckoutCart checkoutCart)
        {
            if (checkoutCart.IsPaid)
            {
                throw new CheckoutCartAlreadyPaidException(checkoutCart.Id);
            }
        }

    }
}
