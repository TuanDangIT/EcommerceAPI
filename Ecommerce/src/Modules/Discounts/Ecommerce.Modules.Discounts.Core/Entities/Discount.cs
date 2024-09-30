using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Ecommerce.Modules.Discounts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public class Discount
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        //public DiscountType Type { get; set; }
        public bool IsActive { get; private set; } = false;
        public int Redemptions { get; private set; } = 0;
        public string StripePromotionCodeId { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Coupon Coupon { get; set; } = default!;
        public int CouponId { get; set; }
        public Discount(string code, string stripePromotionCodeId, DateTime? expiresAt, DateTime createdAt)
        {
            Code = code;
            CreatedAt = createdAt;
            StripePromotionCodeId = stripePromotionCodeId;
            if(expiresAt is not null && expiresAt < TimeProvider.System.GetUtcNow().UtcDateTime)
            {
                throw new DiscountInvalidExpiresAtDateException((DateTime)expiresAt!);
            }
            ExpiresAt = expiresAt;
        }
        public Discount(string code, DateTime createdAt)
        {
            Code = code;
            CreatedAt = createdAt;
        }
        public Discount()
        {
            
        }
        public void Activate(DateTime updatedAt)
        {
            UpdatedAt = updatedAt;
            IsActive = true;
        }
        public void Deactive(DateTime updatedAt)
        {
            UpdatedAt = updatedAt;
            IsActive = false;
        }
        public void Redeem(DateTime updatedAt)
        {
            UpdatedAt = updatedAt;
            Redemptions++;
        }
    }
}
