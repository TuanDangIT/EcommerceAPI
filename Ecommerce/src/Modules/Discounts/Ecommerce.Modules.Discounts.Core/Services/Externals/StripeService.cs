using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Exceptions;
using Ecommerce.Shared.Infrastructure.Stripe;
using Microsoft.Extensions.Logging;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace Ecommerce.Modules.Discounts.Core.Services.Externals
{
    internal class StripeService : IPaymentProcessorService
    {
        private readonly StripeOptions _stripeOptions;
        private readonly ILogger<StripeService> _logger;
        private readonly RequestOptions _requestOptions;
        private const string _couponDuration = "forever";

        public StripeService(StripeOptions stripeOptions, ILogger<StripeService> logger)
        {
            _stripeOptions = stripeOptions;
            _logger = logger;
            _requestOptions = new RequestOptions()
            {
                ApiKey = _stripeOptions.ApiKey
            };
        }

        public async Task<string> CreateCouponAsync(NominalCouponCreateDto dto, CancellationToken cancellationToken = default)
        {
            var options = new CouponCreateOptions()
            {
                Name = dto.Name,
                Duration = _couponDuration,
                AmountOff = (long)(dto.NominalValue * 100),
                Currency = _stripeOptions.Currency
            };
            var couponService = new Stripe.CouponService();
            var coupon = await couponService.CreateAsync(options, _requestOptions, cancellationToken);
            if(coupon.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeFailedRequestException(coupon.StripeResponse.Content);
            }
            _logger.LogDebug("Stripe nominal coupon was created: {@coupon}.", dto);
            return coupon.Id;
        }

        public async Task<string> CreateCouponAsync(PercentageCouponCreateDto dto, CancellationToken cancellationToken = default)
        {
            var options = new CouponCreateOptions()
            {
                Name = dto.Name,
                Duration = _couponDuration,
                PercentOff = dto.Percent * 100,
            };
            var couponService = new Stripe.CouponService();
            var coupon = await couponService.CreateAsync(options, _requestOptions, cancellationToken);
            if (coupon.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeFailedRequestException(coupon.StripeResponse.Content);
            }
            _logger.LogDebug("Stripe percentage coupon was created: {@coupon}.", dto);
            return coupon.Id;
        }
        public async Task DeleteCouponAsync(string stripeCouponId, CancellationToken cancellationToken = default)
        {
            var couponService = new Stripe.CouponService();
            var coupon = await couponService.DeleteAsync(stripeCouponId, requestOptions: _requestOptions, cancellationToken: cancellationToken);
            if (coupon.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeFailedRequestException(coupon.StripeResponse.Content);
            }
            _logger.LogDebug("Stripe coupon was deleted: {stripeCouponId}.", stripeCouponId);
        }
        public async Task UpdateCouponName(string stripeCouponId, string name, CancellationToken cancellationToken = default)
        {
            var options = new CouponUpdateOptions()
            {
                Name = name,
            };
            var couponService = new Stripe.CouponService();
            var coupon = await couponService.UpdateAsync(stripeCouponId, options, _requestOptions, cancellationToken);
            if (coupon.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeFailedRequestException(coupon.StripeResponse.Content);
            }
            _logger.LogDebug("Stripe coupon name was updated: {stripeCouponId} to {name}.", stripeCouponId, name);
        }
        public async Task<string> CreateDiscountAsync(string stripeCouponId, DiscountCreateDto dto, CancellationToken cancellationToken = default)
        {
            var options = new PromotionCodeCreateOptions()
            {
                Active = false,
                Code = dto.Code,
                ExpiresAt = dto.ExpiresDate,
                Coupon = stripeCouponId
            };
            var promotionCodeService = new PromotionCodeService();
            var promotionCode = await promotionCodeService.CreateAsync(options, _requestOptions, cancellationToken);
            if (promotionCode.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeFailedRequestException(promotionCode.StripeResponse.Content);
            }
            _logger.LogDebug("Stripe discount: {@discount} was created for coupon: {stripeCouponId}.", dto, stripeCouponId);
            return promotionCode.Id;
        }
        public async Task ActivateDiscountAsync(string stripePromotionCodeId, CancellationToken cancellationToken = default)
            => await ChangeDiscountStatus(stripePromotionCodeId, true, cancellationToken);

        public Task DeactivateDiscountAsync(string stripePromotionCodeId, CancellationToken cancellationToken = default)
            => ChangeDiscountStatus(stripePromotionCodeId, false, cancellationToken);
        private async Task ChangeDiscountStatus(string stripePromotionCodeId, bool isActive, CancellationToken cancellationToken = default)
        {
            var options = new PromotionCodeUpdateOptions()
            {
                Active = isActive
            };
            var promotionCodeService = new PromotionCodeService();
            var promotionCode = await promotionCodeService.UpdateAsync(stripePromotionCodeId, options, _requestOptions, cancellationToken);
            if (promotionCode.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeFailedRequestException(promotionCode.StripeResponse.Content);
            }
            _logger.LogDebug("Stripe discount: {stripePromotionCodeId} status: {status} was updated.", stripePromotionCodeId, isActive);
        }
    }
}
