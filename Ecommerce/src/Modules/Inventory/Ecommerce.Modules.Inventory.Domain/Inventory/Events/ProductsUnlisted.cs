using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Events
{
    public sealed record class ProductsUnlisted(IEnumerable<Guid> ProductIds) : IDomainEvent;
}
