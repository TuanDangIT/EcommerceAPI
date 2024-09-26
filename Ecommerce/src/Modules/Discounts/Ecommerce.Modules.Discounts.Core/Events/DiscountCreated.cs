using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Events
{
    public sealed record class DiscountCreated(string Code, string Type, decimal Value) : IEvent;
}
