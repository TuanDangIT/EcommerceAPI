using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Events;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Modules.Carts.Core.Mappings;
using Ecommerce.Modules.Carts.Core.Services.Externals;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services
{
    internal class CheckoutCartService : ICheckoutCartService
    {
        private readonly ICartsDbContext _dbContext;
        private readonly IStripeService _stripeService;
        private readonly IMessageBroker _messageBroker;

        public CheckoutCartService(ICartsDbContext dbContext, IStripeService stripeService, IMessageBroker messageBroker)
        {
            _dbContext = dbContext;
            _stripeService = stripeService;
            _messageBroker = messageBroker;
        }

        public async Task<CheckoutCartDto?> GetAsync(Guid checkoutCartId)
        {
            var checkoutCart = await _dbContext.CheckoutCarts
                .Include(cc => cc.Payment)
                .Include(cc => cc.Products)
                .ThenInclude(cp => cp.Product)
                .SingleOrDefaultAsync(cc => cc.Id== checkoutCartId);
            return checkoutCart?.AsDto();
        }

        public async Task<CheckoutStripeSessionDto> PlaceOrderAsync(Guid checkoutCartId)
        {
            var checkoutCart = await GetOrThrowIfNull(checkoutCartId);
            if(checkoutCart.Shipment is null || checkoutCart.Payment is null)
            {
                throw new CheckoutCartInvalidPlaceOrderException();
            }
            var dto = await _stripeService.Checkout(checkoutCart);
            checkoutCart.SetStripeSessionId(dto.SessionId);
            //foreach (var cartProduct in checkoutCart.Products)
            //{
            //    var product = cartProduct.Product;
            //    if(product.Quantity < cartProduct.Quantity)
            //    {

            //    }
            //    product.DecreaseQuantity(cartProduct.Quantity);
            //}
            await _dbContext.SaveChangesAsync();
            return dto;
        }
        public async Task SetCustomer(Guid checkoutCartId, CustomerDto customerDto)
        {
            var checkoutCart = await GetOrThrowIfNull(checkoutCartId);
            checkoutCart.SetCustomer(new Customer(
                customerDto.FirstName,
                customerDto.LastName,
                customerDto.Email,
                customerDto.PhoneNumber
                ));
            await _dbContext.SaveChangesAsync();
        }
        public async Task SetPaymentAsync(Guid checkoutCartId, Guid paymentId)
        {
            var checkoutCart = await GetOrThrowIfNull(checkoutCartId);
            var payment = await _dbContext.Payments
                .SingleOrDefaultAsync(p => p.Id == paymentId);
            if(payment is null)
            {
                throw new PaymentNotFoundException(paymentId);
            }
            checkoutCart.SetPayment(payment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SetShipmentAsync(Guid checkoutCartId, ShipmentDto shipmentDto)
        {
            var checkoutCart = await GetOrThrowIfNull(checkoutCartId);
            checkoutCart.SetShipment(new Shipment(
                shipmentDto.City,
                shipmentDto.PostalCode,
                shipmentDto.StreetName,
                shipmentDto.StreetNumber,
                shipmentDto.AparmentNumber
                ));
            await _dbContext.SaveChangesAsync();
        }
        public async Task SetAdditionalInformation(Guid checkoutCartId, string additionalInformation)
        {
            var checkoutCart = await GetOrThrowIfNull(checkoutCartId);
            checkoutCart.SetAdditionalInformation(additionalInformation);
            await _dbContext.SaveChangesAsync();
        }
        public async Task SetCheckoutCartDetails(Guid checkoutCartId, CheckoutCartSetDetailsDto checkoutCartSetDetailsDto)
        {
            var checkoutCart = await GetOrThrowIfNull(checkoutCartId);
            var shipmentDto = checkoutCartSetDetailsDto.ShipmentDto;
            var customerDto = checkoutCartSetDetailsDto.CustomerDto;
            var paymentId = checkoutCartSetDetailsDto.PaymentId;
            var additionalInformation = checkoutCartSetDetailsDto.AdditionalInformation;
            checkoutCart.SetCustomer(new Customer(
                customerDto.FirstName,
                customerDto.LastName,
                customerDto.Email,
                customerDto.PhoneNumber
                ));
            checkoutCart.SetShipment(new Shipment(
                shipmentDto.City,
                shipmentDto.PostalCode,
                shipmentDto.StreetName,
                shipmentDto.StreetNumber,
                shipmentDto.AparmentNumber
                ));
            var payment = await _dbContext.Payments
                .SingleOrDefaultAsync(p => p.Id == paymentId);
            if (payment is null)
            {
                throw new PaymentNotFoundException(paymentId);
            }
            checkoutCart.SetPayment(payment);
            if(additionalInformation is not null)
            {
                checkoutCart.SetAdditionalInformation(additionalInformation);
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task HandleCheckoutSessionCompleted(Session? session)
        {
            if(session is null)
            {
                throw new NullReferenceException();
            }
            var sessionId = session.Id;
            var checkoutCart = await _dbContext.CheckoutCarts
                .Include(cc => cc.Products)
                .ThenInclude(cp => cp.Product)
                .Include(cc => cc.Payment)
                .SingleOrDefaultAsync(cc => cc.StripeSessionId == sessionId);
            if(checkoutCart is null)
            {
                throw new CheckoutCartNotFoundException();
            }
            checkoutCart.SetPaid();
            await _dbContext.SaveChangesAsync();
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
                City = checkoutCart.Shipment!.City,
                PostalCode = checkoutCart.Shipment.PostalCode,
                StreetName = checkoutCart.Shipment.StreetName,
                StreetNumber = checkoutCart.Shipment.StreetNumber,
                ApartmentNumber = checkoutCart.Shipment.AparmentNumber,
                PaymentMethod = checkoutCart.Payment!.PaymentMethod.ToString()
            });
        }
        private async Task<CheckoutCart> GetOrThrowIfNull(Guid checkoutCartId)
        {
            var checkoutCart = await _dbContext.CheckoutCarts
                .Include(cc => cc.Products)
                .ThenInclude(cp => cp.Product)
                .Include(cc => cc.Payment)
                .SingleOrDefaultAsync(cc => cc.Id == checkoutCartId);
            if (checkoutCart is null)
            {
                throw new CheckoutCartNotFoundException(checkoutCartId);
            }
            return checkoutCart;
        }
    }
}
