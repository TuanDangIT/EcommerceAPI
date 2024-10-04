using Ecommerce.Modules.Carts.Core.DAL.Mappings;
using Ecommerce.Modules.Carts.Core.DTO;
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
        private readonly decimal _defaultDeliveryPrice = 15;

        public StripeService(StripeOptions stripeOptions)
        {
            _stripeOptions = stripeOptions;
            _requestOptions = new RequestOptions()
            {
                ApiKey = _stripeOptions.ApiKey
            };
        }
        public async Task<CheckoutStripeSessionDto> CheckoutAsync(CheckoutCart checkoutCart)
        {
            var lineItems = new List<SessionLineItemOptions>();
            foreach (var product in checkoutCart.Products)
            {
                var discount = checkoutCart.Discount;
                lineItems.Add(new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        //UnitAmount = (long)Math.Truncate(product.Product.Price),
                        UnitAmountDecimal = product.Product.Price * 100,
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
                PaymentMethodTypes =
                [
                    checkoutCart.Payment is not null ? checkoutCart.Payment.PaymentMethod.ToString() : throw new PaymentNotSetException(),
                ],
                LineItems = lineItems,
                Mode = _stripeOptions.Mode,
                ShippingOptions = new List<SessionShippingOptionOptions>()
                {
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
                }
                //InvoiceCreation = new SessionInvoiceCreationOptions()
                //{
                //    Enabled = true,
                //    InvoiceData = new SessionInvoiceCreationInvoiceDataOptions()
                //    {
                //        Description = "some description",
                //        Issuer = new SessionInvoiceCreationInvoiceDataIssuerOptions()
                //        {
                //            Type = "self"
                //        }
                        
                //    }
                //}
            };
            if(checkoutCart.Discount is not null)
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
            var session = await sessionService.CreateAsync(sessionOptions, _requestOptions);
            return session.AsDto();
        }
    }
}
