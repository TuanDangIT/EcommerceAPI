using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Events
{
    public sealed record class OrderReturned(string ReasonForReturn, Guid OrderId, IEnumerable<Product> Products, bool IsFullReturn, DateTime CreatedAt) : IDomainEvent;
}
