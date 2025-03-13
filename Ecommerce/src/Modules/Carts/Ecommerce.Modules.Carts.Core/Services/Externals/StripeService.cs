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
    internal class StripeService : IPaymentProcessorService
    {
        private readonly StripeOptions _stripeOptions;
        private readonly ILogger<StripeService> _logger;
        private readonly RequestOptions _requestOptions;

        public StripeService(StripeOptions stripeOptions, ILogger<StripeService> logger)
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
            if(checkoutCart.Payment is null)
            {
                throw new PaymentNotSetException();
            }
            if(checkoutCart.Shipment is null)
            {
                throw new ShipmentNotSetException();
            }
            var lineItems = CreateLineItems(checkoutCart);
            var sessionOptions = CreateSessionOptions(checkoutCart, lineItems);
            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(sessionOptions, _requestOptions, cancellationToken);
            if(session.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeFailedRequestException(session.StripeResponse.Content);
            }
            _logger.LogDebug("Stripe session was created for checkout cart: {checkoutCartId}.", checkoutCart.Id);
            return session.AsDto();
        }
        private List<SessionLineItemOptions> CreateLineItems(CheckoutCart checkoutCart)
        {
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var product in checkoutCart.Products)
            {
                var productPrice = CalculateProductPrice(product, checkoutCart.Discount);

                var lineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = productPrice * 100,
                        Currency = _stripeOptions.Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Product.Name,
                            Metadata = new Dictionary<string, string>
                            {
                                { "SKU", product.Product.SKU }
                            }
                        }
                    },
                    Quantity = product.Quantity,
                };

                if (!string.IsNullOrEmpty(product.Product.ImagePathUrl))
                {
                    lineItem.PriceData.ProductData.Images = [_stripeOptions.BlobStorageUrl + product.Product.ImagePathUrl];
                }

                lineItems.Add(lineItem);
            }
            return lineItems;
        }
        private decimal CalculateProductPrice(CartProduct product, Entities.Discount? discount)
        {
            if (discount?.SKU == product.Product.SKU)
            {
                return product.Product.Price - discount.Value;
            }
            return product.Product.Price;
        }
        private SessionCreateOptions CreateSessionOptions(CheckoutCart checkoutCart, List<SessionLineItemOptions> lineItems)
        {
            var sessionOptions = new SessionCreateOptions
            {
                SuccessUrl = "https://localhost:7089/api", // TODO: Replace with configurable URLs
                CancelUrl = "https://localhost:7089/api",
                PaymentMethodTypes = [checkoutCart.Payment!.PaymentMethod.ToString() ?? throw new NullReferenceException()],
                LineItems = lineItems,
                Mode = _stripeOptions.Mode,
                ShippingOptions = [CreateShippingOption(checkoutCart)]
            };
            var discount = checkoutCart.Discount;
            if (discount is not null && !string.IsNullOrEmpty(discount.StripePromotionCodeId))
            {
                sessionOptions.Discounts = [new SessionDiscountOptions
                {
                    PromotionCode = discount?.StripePromotionCodeId
                }];
            }
            return sessionOptions;
        }

        private SessionShippingOptionOptions CreateShippingOption(CheckoutCart checkoutCart)
        {
            return new SessionShippingOptionOptions
            {
                ShippingRateData = new SessionShippingOptionShippingRateDataOptions
                {
                    DisplayName = "Kurier InPost", // TODO: Make configurable
                    FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                    {
                        Amount = (long)(checkoutCart.Shipment!.Price * 100),
                        Currency = _stripeOptions.Currency,
                    },
                    Type = "fixed_amount"
                }
            };
        }
    }
}
