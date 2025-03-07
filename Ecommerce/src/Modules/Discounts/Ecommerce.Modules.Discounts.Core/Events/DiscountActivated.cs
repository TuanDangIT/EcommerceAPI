using Ecommerce.Modules.Discounts.Core.DataAnnotations;
using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Events
{
    public sealed record class DiscountActivated(string Code, string Type, string StripePromotionCodeId, decimal Value, decimal RequiredCartTotalValue, DateTime? ExpiresAt) : IEvent;
}

