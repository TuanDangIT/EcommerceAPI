using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Events
{
    public sealed record class OrderReturned(Guid OrderId, Guid? CustomerId, string FirstName, string Email, string ReasonForReturn, IEnumerable<Product> Products, bool IsFullReturn) : IDomainEvent;
}
