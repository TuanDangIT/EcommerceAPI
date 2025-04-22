using Ecommerce.Modules.Discounts.Core.DTO;
using Ecommerce.Modules.Discounts.Core.Services.Externals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Tests.Integration
{
    internal class FakeStripeService : IPaymentProcessorService
    {
        public Task ActivateDiscountAsync(string stripePromotionCodeId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateCouponAsync(NominalCouponCreateDto dto, CancellationToken cancellationToken = default)
        {
            return Task.FromResult("stripe-coupon-id");
        }

        public Task<string> CreateCouponAsync(PercentageCouponCreateDto dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateDiscountAsync(string stripeCouponId, DiscountCreateDto dto, CancellationToken cancellationToken = default)
        {
            return Task.FromResult("stripe-promotion-id");
        }

        public Task DeactivateDiscountAsync(string stripePromotionCodeId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCouponAsync(string stripeCouponId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCouponName(string stripeCouponId, string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
