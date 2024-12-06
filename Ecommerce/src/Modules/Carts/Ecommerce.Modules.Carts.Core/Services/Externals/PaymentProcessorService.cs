using Ecommerce.Modules.Carts.Core.DAL.Mappings;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Shared.Infrastructure.Stripe;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services.Externals
{
    internal class PaymentProcessorService : IPaymentProcessorService
    {
        private readonly StripeOptions _stripeOptions;
        private readonly ILogger<PaymentProcessorService> _logger;
        private readonly RequestOptions _requestOptions;
        private readonly decimal _defaultDeliveryPrice = 15;

        public PaymentProcessorService(StripeOptions stripeOptions, ILogger<PaymentProcessorService> logger)
        {
            _stripeOptions = stripeOptions;
            _logger = logger;
            _requestOptions = new RequestOptions()
            {
                ApiKey = _stripeOptions.ApiKey
            };
        }
        public async Task<CheckoutStripeSessionDto> CheckoutAsync(CheckoutCart checkoutCart, CancellationToken cancellationToken = default)
        {
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var product in checkoutCart.Products)
            {
                var discount = checkoutCart.Discount;
                var productPrice = product.Product.Price;
                if(discount?.SKU == product.Product.SKU)
                {
                    productPrice = product.Product.Price - discount.Value;
                }
                lineItems.Add(new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmountDecimal = productPrice * 100,
                        Currency = _stripeOptions.Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = product.Product.Name,
                            Images = [_stripeOptions.BlobStorageUrl + product.Product.ImagePathUrl],
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
                //Urls are just temporary here
                SuccessUrl = "https://localhost:7089/api",
                CancelUrl = "https://localhost:7089/api",
                PaymentMethodTypes =
                [
                    checkoutCart.Payment is not null ? checkoutCart.Payment.PaymentMethod.ToString() : throw new PaymentNotSetException(),
                ],
                LineItems = lineItems,
                Mode = _stripeOptions.Mode,
                ShippingOptions =
                [
                    new SessionShippingOptionOptions()
                    {
                        ShippingRateData = new SessionShippingOptionShippingRateDataOptions()
                        {
                            DisplayName = "Kurier InPost",
                            FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions()
                            {
                                Amount = (long)(_defaultDeliveryPrice * 100),
                                Currency = _stripeOptions.Currency,
                            },
                            Type = "fixed_amount"
                        }
                    }
                ]
            };
            if (checkoutCart.Discount is not null && checkoutCart.Discount.StripePromotionCodeId is not null)
            {
                sessionOptions.Discounts =
                [
                    new()
                    {
                        PromotionCode = checkoutCart.Discount.StripePromotionCodeId
                    }
                ];
            }
            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(sessionOptions, _requestOptions, cancellationToken);
            if(session.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeFailedRequestException();
            }
            _logger.LogInformation("Session created: {session}", session);
            return session.AsDto();
        }
    }
}
