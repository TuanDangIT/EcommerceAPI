using Ecommerce.Modules.Inventory.Application.Inventory.Events;
using Ecommerce.Modules.Inventory.Domain.Inventory.Events;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Services
{
    internal class InventoryEventMapper : IInventoryEventMapper
    {
        public IMessage Map(IDomainEvent @event)
            => @event switch
            {
                Domain.Inventory.Events.ProductsListed e => new Events.ProductsListed(e.Products.Select(p => new { p.Id, p.SKU, p.Name, p.Price, p.Quantity, ImagePathUrl = p.Images.First(i => i.Order == 1).ImageUrlPath })),
                Domain.Inventory.Events.ProductsUnlisted e => new Events.ProductsUnlisted(e.ProductIds),
                _ => throw new ArgumentException(nameof(@event)),
            };
    }
}
