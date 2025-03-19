using Ecommerce.Modules.Orders.Application.Shared.Stripe;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Infrastructure.Stripe;
using Microsoft.Extensions.Logging;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Stripe
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
        public async Task RefundAsync(Domain.Orders.Entities.Order order, CancellationToken cancellationToken = default)
        {
            var refundOptions = new RefundCreateOptions()
            {
                PaymentIntent = order.StripePaymentIntentId,
            };
            var refundService = new RefundService();
            var refund = await refundService.CreateAsync(refundOptions, _requestOptions, cancellationToken);
            if(refund.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeFailedRequestException(refund.StripeResponse.Content);
            }
            _logger.LogDebug("Money was refunded for order: {orderId} on stripe.", order.Id);
        }
        public async Task RefundAsync(Domain.Orders.Entities.Order order, decimal amount, CancellationToken cancellationToken = default)
        {
            var refundOptions = new RefundCreateOptions()
            {
                PaymentIntent = order.StripePaymentIntentId,
                //In cents or the charge currency’s smallest currency unit
                Amount = (long)(amount * 100),
            };
            var refundService = new RefundService();
            var refund = await refundService.CreateAsync(refundOptions, _requestOptions, cancellationToken);
            if (refund.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeFailedRequestException(refund.StripeResponse.Content);
            }
            _logger.LogDebug("Money was refunded for order: {orderId} on stripe.", order.Id);
        }
    }
}
