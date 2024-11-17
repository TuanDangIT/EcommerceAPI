using Ecommerce.Modules.Orders.Application.Shared.Stripe;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Infrastructure.Stripe;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Stripe
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
        public async Task Refund(Domain.Orders.Entities.Order order)
        {
            var refundOptions = new RefundCreateOptions()
            {
                PaymentIntent = order.StripePaymentIntentId,
                //Currency = _stripeOptions.Currency
            };
            var refundService = new RefundService();
            await refundService.CreateAsync(refundOptions, _requestOptions);
        }
        public async Task Refund(Domain.Orders.Entities.Order order, decimal amount)
        {
            var refundOptions = new RefundCreateOptions()
            {
                PaymentIntent = order.StripePaymentIntentId,
                //In cents or the charge currency’s smallest currency unit
                Amount = (long)(amount * 100),
                //Currency = _stripeOptions.Currency.ToLower()
            };
            var refundService = new RefundService();
            await refundService.CreateAsync(refundOptions, _requestOptions);
        }
    }
}
