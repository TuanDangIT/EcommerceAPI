using Ecommerce.Modules.Orders.Application.Orders.Events;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Services
{
    internal class OrdersEventMapper : IOrdersEventMapper
    {

        public OrdersEventMapper()
        {
        }
        public IMessage Map(IDomainEvent @event)
            => @event switch
            {
                Domain.Orders.Events.OrderReturned e => new OrderReturned(e.OrderId, e.CustomerId, e.FirstName, e.Email, e.Products.Select(p => new { p.SKU, p.Name, p.Price, p.Quantity })),
                Domain.Orders.Events.ComplaintSubmitted e => new ComplaintSubmitted(e.OrderId, e.CustomerId, e.FirstName, e.Email, e.Title, e.CreatedAt),
                _ => throw new ArgumentException(nameof(@event)),
            };
    }
}
