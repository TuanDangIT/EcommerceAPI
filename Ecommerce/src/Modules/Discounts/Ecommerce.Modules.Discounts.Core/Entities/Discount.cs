﻿using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Ecommerce.Modules.Discounts.Core.Entities.Exceptions;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public class Discount : BaseEntity<int>, IAuditable
    {
        public string Code { get; private set; } = string.Empty;
        //public DiscountType Type { get; set; }
        public bool IsActive { get; private set; } = false;
        public int Redemptions { get; private set; } = 0;
        public string StripePromotionCodeId { get; private set; } = string.Empty;
        public DateTime? ExpiresAt { get; private set; }
        public Coupon Coupon { get; private set; } = default!;
        public int CouponId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Discount(string code, string stripePromotionCodeId, DateTime? expiresAt)
        {
            Code = code;
            StripePromotionCodeId = stripePromotionCodeId;
            if(expiresAt is not null && expiresAt < TimeProvider.System.GetUtcNow().UtcDateTime)
            {
                throw new DiscountInvalidExpiresAtDateException((DateTime)expiresAt!);
            }
            ExpiresAt = expiresAt;
        }
        public Discount(string code)
        {
            Code = code;
        }
        public Discount()
        {
            
        }
        public void Activate()
            => IsActive = true;
        public void Deactive()
            => IsActive = false;
        public void Redeem()
            => Redemptions++;
    }
}
