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
    internal class EventMapper : IEventMapper
    {
        public IMessage Map(IDomainEvent @event)
            => @event switch
            {
                Domain.Inventory.Events.ProductListed e => new Events.ProductListed(e.Products.Select(p => new { p.Id, p.Name, p.Price, ImagePathUrl = p.Images.First() })),
                Domain.Inventory.Events.ProductUnlisted e => new Events.ProductUnlisted(e.ProductIds),
                _ => throw new ArgumentException(nameof(@event)),
            };
    }
}
