using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Events
{
    public sealed record class ReturnProductAdded(Guid OrderId, string SKU, int Quantity) : IDomainEvent;
}
