﻿using Ecommerce.Modules.Carts.Core.DAL;
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
    internal class CheckoutCartService : ICheckoutCartService
    {
        private readonly ICartsDbContext _dbContext;

        public CheckoutCartService(ICartsDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task PlaceOrderAsync(Guid checkoutCartId)
        {
            var checkoutCart = await GetOrThrowIfNull(checkoutCartId);
        }

        public async Task SetPaymentAsync(Guid paymentId, Guid checkoutCartId)
        {
            var checkoutCart = await GetOrThrowIfNull(checkoutCartId);
            var payment = await _dbContext.Payments
                .SingleOrDefaultAsync(p => p.Id == paymentId);
            if(payment is null)
            {
                throw new PaymentNotFoundException(paymentId);
            }
            checkoutCart.SetPayment(payment);
            var rowsChange = await _dbContext.SaveChangesAsync();
            if(rowsChange != 1)
            {
                throw new PaymentNotUpdatedException();
            }
        }

        public async Task SetShipment(ShipmentDto shipmentDto, Guid checkoutCartId)
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
            var rowsChanged = await _dbContext.SaveChangesAsync();
            if(rowsChanged != 1)
            {
                throw new ShipmentNotUpdatedException();
            }
        }
        private async Task<CheckoutCart> GetOrThrowIfNull(Guid checkoutCartId)
        {
            var checkoutCart = await _dbContext.CheckoutCarts
                .SingleOrDefaultAsync(cc => cc.Id == checkoutCartId);
            if (checkoutCart is null)
            {
                throw new CheckoutCartNotFoundException(checkoutCartId);
            }
            return checkoutCart;
        }
    }
}
