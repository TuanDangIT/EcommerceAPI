using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Events.Externals
{
    public sealed record class DiscountCodeRedeemed(string Code) : IEvent;
}
