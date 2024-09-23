using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Events
{
    public sealed record class OrderCancelled(IEnumerable<object> Products) : IEvent;
}
