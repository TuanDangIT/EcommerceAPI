using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services
{
    public interface ICheckoutCartService
    {
        Task<CheckoutCartDto?> GetAsync(Guid checkoutCartId);
        Task SetCustomer(Guid checkoutCartId, CustomerDto customerDto);
        Task SetPaymentAsync(Guid checkoutCartId, Guid paymentId);
        Task SetShipmentAsync(Guid checkoutCartId, ShipmentDto shipmentDto);
        Task<CheckoutStripeSessionDto> PlaceOrderAsync(Guid checkoutCartId);
        Task SetCheckoutCartDetails(Guid checkoutCartId, CheckoutCartSetDetailsDto checkoutCartSetDetailsDto);
        Task HandleCheckoutSessionCompleted(Session? session);
    }
}
