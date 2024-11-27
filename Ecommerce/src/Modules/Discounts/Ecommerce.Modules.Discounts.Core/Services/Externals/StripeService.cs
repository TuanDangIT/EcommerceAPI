using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Shared.Infrastructure.Stripe;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace Ecommerce.Modules.Discounts.Core.Services.Externals
{
    internal class StripeService : IStripeService
    {
        private readonly StripeOptions _stripeOptions;
        private readonly RequestOptions _requestOptions;
        private const string _couponDuration = "forever";

        public StripeService(StripeOptions stripeOptions)
        {
            _stripeOptions = stripeOptions;
            _requestOptions = new RequestOptions()
            {
                ApiKey = _stripeOptions.ApiKey
            };
        }

        public async Task<string> CreateCouponAsync(NominalCouponCreateDto dto)
        {
            var options = new CouponCreateOptions()
            {
                Name = dto.Name,
                Duration = _couponDuration,
                AmountOff = (long)(dto.NominalValue * 100),
                Currency = _stripeOptions.Currency
            };
            var couponService = new Stripe.CouponService();
            var coupon = await couponService.CreateAsync(options, _requestOptions);
            if(coupon.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeException("Failed to process Stripe request.");
            }
            return coupon.Id;
        }

        public async Task<string> CreateCouponAsync(PercentageCouponCreateDto dto)
        {
            var options = new CouponCreateOptions()
            {
                Name = dto.Name,
                Duration = _couponDuration,
                PercentOff = dto.Percent * 100,
            };
            var couponService = new Stripe.CouponService();
            var coupon = await couponService.CreateAsync(options, _requestOptions);
            if (coupon.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeException("Failed to process Stripe request.");
            }
            return coupon.Id;
        }
        public async Task DeleteCouponAsync(string stripeCouponId)
        {
            var couponService = new Stripe.CouponService();
            var coupon = await couponService.DeleteAsync(stripeCouponId, requestOptions: _requestOptions);
            if (coupon.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeException("Failed to process Stripe request.");
            }
        }
        public async Task UpdateCouponName(string stripeCouponId, string name)
        {
            var options = new CouponUpdateOptions()
            {
                Name = name,
            };
            var couponService = new Stripe.CouponService();
            var coupon = await couponService.UpdateAsync(stripeCouponId, options, _requestOptions);
            if (coupon.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeException("Failed to process Stripe request.");
            }
        }
        public async Task<string> CreateDiscountAsync(string stripeCouponId, DiscountCreateDto dto)
        {
            var options = new PromotionCodeCreateOptions()
            {
                Active = false,
                Code = dto.Code,
                ExpiresAt = dto.EndingDate,
                Coupon = stripeCouponId
            };
            var promotionCodeService = new PromotionCodeService();
            var promotionCode = await promotionCodeService.CreateAsync(options, _requestOptions);
            if (promotionCode.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeException("Failed to process Stripe request.");
            }
            return promotionCode.Id;
        }
        public async Task ActivateDiscountAsync(string stripePromotionCodeId)
            => await ChangeDiscountStatus(stripePromotionCodeId, true);

        public Task DeactivateDiscountAsync(string stripePromotionCodeId)
            => ChangeDiscountStatus(stripePromotionCodeId, false);
        private async Task ChangeDiscountStatus(string stripePromotionCodeId, bool isActive)
        {
            var options = new PromotionCodeUpdateOptions()
            {
                Active = isActive
            };
            var promotionCodeService = new PromotionCodeService();
            var promotionCode = await promotionCodeService.UpdateAsync(stripePromotionCodeId, options, _requestOptions);
            if (promotionCode.StripeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new StripeException("Failed to process Stripe request.");
            }
        }
    }
}
