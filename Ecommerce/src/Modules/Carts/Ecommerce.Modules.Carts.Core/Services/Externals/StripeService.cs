using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Modules.Carts.Core.Mappings;
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
        private const string Mode = "payment";
        private const string BlobStorageUrl = "http://localhost:10000";

        public StripeService(StripeOptions stripeOptions)
        {
            _stripeOptions = stripeOptions;
            _requestOptions = new RequestOptions()
            {
                ApiKey = _stripeOptions.ApiKey
            };
        }
        public async Task<CheckoutStripeSessionDto> Checkout(CheckoutCart checkoutCart)
        {
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var product in checkoutCart.Products)
            {
                lineItems.Add(new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        //UnitAmount = (long)Math.Truncate(product.Product.Price),
                        UnitAmountDecimal = product.Product.Price,
                        Currency = Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = product.Product.Name,
                            //Images = new List<string>() { product.Product.ImagePathUrl }
                            Images = new List<string>() { BlobStorageUrl + product.Product.ImagePathUrl }
                        }
                    },
                    Quantity = product.Quantity,
                });
            }
            var a = new StripeClient();
            var sessionOptions = new SessionCreateOptions()
            {
                SuccessUrl = "https://localhost:7089/api",
                CancelUrl = "https://localhost:7089/api",
                PaymentMethodTypes = new List<string>
                {
                    checkoutCart.Payment is not null ? checkoutCart.Payment.PaymentMethod.ToString() : throw new PaymentNotSetException()
                },
                LineItems = lineItems,
                Mode = Mode
            };
            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(sessionOptions, _requestOptions);
            return session.AsDto();
        }
    }
}
