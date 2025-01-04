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
        Task<CheckoutCartDto?> GetAsync(Guid checkoutCartId, CancellationToken cancellationToken = default);
        Task SetCustomerAsync(Guid checkoutCartId, CustomerDto customerDto, CancellationToken cancellationToken = default);
        Task SetPaymentAsync(Guid checkoutCartId, Guid paymentId, CancellationToken cancellationToken = default);
        Task SetShipmentAsync(Guid checkoutCartId, ShipmentDto shipmentDto, CancellationToken cancellationToken = default);
        Task SetAdditionalInformationAsync(Guid checkoutCartId, string additionalInformation, CancellationToken cancellationToken = default);
        Task<CheckoutStripeSessionDto> PlaceOrderAsync(Guid checkoutCartId, CancellationToken cancellationToken = default);
        //Task AddDiscountAsync(Guid checkoutCartId, string code, CancellationToken cancellationToken = default);
        //Task RemoveDiscountAsync(Guid checkoutCartId, CancellationToken cancellationToken = default);
        Task SetCheckoutCartDetailsAsync(Guid checkoutCartId, CheckoutCartSetDetailsDto checkoutCartSetDetailsDto, CancellationToken cancellationToken = default);
        Task HandleCheckoutSessionCompletedAsync(string? json, string? stripeSignatureHeader);
    }
}
