using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Events
{
    public sealed record class OrderReturned(Guid OrderId, Guid? CustomerId, string FirstName, string Email, IEnumerable<object> Products) : IEvent;
}
