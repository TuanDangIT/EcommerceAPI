﻿using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
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

        public async Task<CheckoutCartDto> GetAsync(Guid checkoutCartId)
        {
            var checkoutCart = await _dbContext.CheckoutCarts
                .Include(cc => cc.Payment)
                .Include(cc => cc.Products)
                .ThenInclude(cp => cp.Product)
                .SingleOrDefaultAsync(cc => cc.Id== checkoutCartId);
            if(checkoutCart is null)
            {
                throw new CheckoutCartNotFoundException(checkoutCartId);
            }
            return checkoutCart.AsDto();
        }

        public async Task<CheckoutStripeSessionDto> PlaceOrderAsync(Guid checkoutCartId)
        {
            var checkoutCart = await GetOrThrowIfNull(checkoutCartId);
            var dto = await _stripeService.Checkout(checkoutCart);
            checkoutCart.SetStripeSessionId(dto.SessionId);
            await _dbContext.SaveChangesAsync();
            return dto;
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
                shipmentDto.AparmentNumber,
                shipmentDto.ReceiverFullName
                ));
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
