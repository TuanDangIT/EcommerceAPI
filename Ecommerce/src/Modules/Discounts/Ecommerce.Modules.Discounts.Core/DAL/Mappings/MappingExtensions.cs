using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DAL.Mappings
{
    public static class MappingExtensions
    {
        public static NominalCouponBrowseDto AsNominalBrowseDto(this NominalCoupon coupon)
            => new()
            {
                Id = coupon.Id,
                StripeCouponId = coupon.StripeCouponId,
                Name = coupon.Name,
                NominalValue = coupon.NominalValue,
                UpdatedAt = coupon.UpdatedAt,
                CreatedAt = coupon.CreatedAt
            };
        public static PercentageCouponBrowseDto AsPercentageBrowseDto(this PercentageCoupon coupon)
            => new()
            {
                Id = coupon.Id,
                StripeCouponId = coupon.StripeCouponId,
                Name = coupon.Name,
                Percent = coupon.Percent,
                UpdatedAt = coupon.UpdatedAt,
                CreatedAt = coupon.CreatedAt
            };
        public static DiscountBrowseDto AsBrowseDto(this Discount discount)
            => new()
            {
                Id = discount.Id,
                Code = discount.Code,
                Redemptions = discount.Redemptions,
                EndingDate = discount.ExpiresAt,
                CreatedAt = discount.CreatedAt
            };
        public static OfferBrowseDto AsBrowseDto(this Offer offer)
            => new()
            {
                Id = offer.Id,
                Price = offer.Price,
                OldPrice = offer.OldPrice,
                Status = offer.Status.ToString(),
                CreatedAt = offer.CreatedAt,
                UpdatedAt = offer.UpdatedAt
            };
        public static OfferDetailsDto AsDetailsDto(this Offer offer)
            => new()
            {
                Id = offer.Id,
                Price = offer.Price,
                OldPrice = offer.OldPrice,
                Difference = offer.Difference,
                Reason = offer.Reason,
                Status = offer.Status.ToString(),
                UpdatedAt = offer.UpdatedAt,
                CreatedAt = offer.CreatedAt
            };
    }
}
