using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events.External
{
    public sealed record class DiscountActivated(string Code, string Type, string StripePromotionCodeId, decimal Value, decimal RequiredCartTotalValue, DateTime? ExpiresAt) : IEvent;
}
