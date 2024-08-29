using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Events
{
    public sealed record class ProductListed(IEnumerable<Product> Products, DateTime ListedAt) : IDomainEvent;
}
