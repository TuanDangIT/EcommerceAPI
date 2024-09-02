﻿using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services
{
    public interface ICheckoutCartService
    {
        Task<CheckoutCartDto> GetAsync(Guid checkoutCartId);
        Task SetPaymentAsync(Guid paymentId, Guid checkoutCartId);
        Task SetShipment(ShipmentDto shipmentDto, Guid checkoutCartId);
        Task PlaceOrderAsync(Guid checkoutCartId);
    }
}
