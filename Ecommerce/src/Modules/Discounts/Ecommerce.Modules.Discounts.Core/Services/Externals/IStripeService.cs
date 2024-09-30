using Ecommerce.Modules.Discounts.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Services.Externals
{
    internal interface IStripeService
    {
        Task<string> CreateCouponAsync(NominalCouponCreateDto dto);
        Task<string> CreateCouponAsync(PercentageCouponCreateDto dto);
        Task UpdateCouponName(string stripeCouponId, string name);
        Task DeleteCouponAsync(string stripeCouponId);
        Task<string> CreateDiscountAsync(string stripeCouponId, DiscountCreateDto dto);
        Task ActivateDiscountAsync(string stripePromotionCodeId);
        Task DeactivateDiscountAsync(string stripePromotionCodeId);
    }
}
