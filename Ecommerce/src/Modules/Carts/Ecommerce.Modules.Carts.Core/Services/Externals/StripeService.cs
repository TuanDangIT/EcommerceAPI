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

        public StripeService(StripeOptions stripeOptions)
        {
            _stripeOptions = stripeOptions;
            _requestOptions = new RequestOptions()
            {
                ApiKey = _stripeOptions.ApiKey
            };
        }
        public async Task<(CheckoutStripeSessionDto Dto, string PaymentIntendId)> Checkout(CheckoutCart checkoutCart)
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
                        Currency = _stripeOptions.Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = product.Product.Name,
                            //Images = new List<string>() { product.Product.ImagePathUrl }
                            Images = new List<string>() { _stripeOptions.BlobStorageUrl + product.Product.ImagePathUrl },
                            Metadata = new Dictionary<string, string>()
                            {
                                { "SKU", product.Product.SKU }
                            }
                        }
                    },
                    Quantity = product.Quantity,
                });
            }
            var sessionOptions = new SessionCreateOptions()
            {
                SuccessUrl = "https://localhost:7089/api",
                CancelUrl = "https://localhost:7089/api",
                PaymentMethodTypes = new List<string>
                {
                    checkoutCart.Payment is not null ? checkoutCart.Payment.PaymentMethod.ToString() : throw new PaymentNotSetException()
                },
                LineItems = lineItems,
                Mode = _stripeOptions.Mode,
            };
            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(sessionOptions, _requestOptions);
            var id = session.PaymentIntentId;
            return (Dto: session.AsDto(), PaymentIntendId: session.PaymentIntentId);
        }
    }
}
