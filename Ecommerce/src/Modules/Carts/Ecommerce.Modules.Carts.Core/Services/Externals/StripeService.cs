using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Shared.Infrastructure.Stripe;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services.Externals
{
    internal class StripeService : IStripeService
    {
        private readonly StripeOptions _stripeOptions;
        private readonly RequestOptions _requestOptions;
        private const string Currency = "USD";

        public StripeService(StripeOptions stripeOptions)
        {
            _stripeOptions = stripeOptions;
            _requestOptions = new RequestOptions()
            {
                ApiKey = _stripeOptions.ApiKey
            };
        }
        public async Task<string> Checkout(CheckoutCart checkoutCart)
        {
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var product in checkoutCart.Products)
            {
                lineItems.Add(new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)Math.Truncate(product.Product.Price),
                        UnitAmountDecimal = (long)(product.Product.Price - Math.Truncate(product.Product.Price)),
                        Currency = Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = product.Product.Name,
                            Images = new List<string>() { product.Product.ImagePathUrl }
                        }
                    },
                    Quantity = product.Quantity,
                });
            }
            var sessionOptions = new SessionCreateOptions()
            {
                PaymentMethodTypes = new List<string>
            {
                checkoutCart.Payment is not null ? checkoutCart.Payment.PaymentMethod.ToString() : throw new PaymentNotSetException()
            },
                LineItems = lineItems,
                Mode = "Payment"
            };
            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(sessionOptions, _requestOptions);
            return session.Url;
        }
    }
}
