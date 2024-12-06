using Ecommerce.Modules.Discounts.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Services.Externals
{
    internal interface IPaymentProcessorService
    {
        Task<string> CreateCouponAsync(NominalCouponCreateDto dto, CancellationToken cancellationToken = default);
        Task<string> CreateCouponAsync(PercentageCouponCreateDto dto, CancellationToken cancellationToken = default);
        Task UpdateCouponName(string stripeCouponId, string name, CancellationToken cancellationToken = default);
        Task DeleteCouponAsync(string stripeCouponId, CancellationToken cancellationToken = default);
        Task<string> CreateDiscountAsync(string stripeCouponId, DiscountCreateDto dto, CancellationToken cancellationToken = default);
        Task ActivateDiscountAsync(string stripePromotionCodeId, CancellationToken cancellationToken = default);
        Task DeactivateDiscountAsync(string stripePromotionCodeId, CancellationToken cancellationToken = default);
    }
}
