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
        public static NominalCouponBrowseDto AsNominalDto(this NominalCoupon coupon)
            => new()
            {
                Name = coupon.Name,
                NominalValue = coupon.NominalValue,
                UpdatedAt = coupon.UpdatedAt,
                CreatedAt = coupon.CreatedAt
            };
        public static PercentageCouponBrowseDto AsPercentageDto(this PercentageCoupon coupon)
            => new()
            {
                Name = coupon.Name,
                Percent = coupon.Percent,
                UpdatedAt = coupon.UpdatedAt,
                CreatedAt = coupon.CreatedAt
            };
        public static DiscountDto AsDto(this Discount discount)
            => new()
            {
                Code = discount.Code,
                EndingDate = discount.ExpiresAt,
                CreatedAt = discount.CreatedAt
            };
    }
}
