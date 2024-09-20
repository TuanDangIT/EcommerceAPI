using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Returns.Entity;
using Ecommerce.Modules.Orders.Domain.Returns.Exception;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Events.Handlers
{
    internal sealed class OrderReturnedHandler : IDomainEventHandler<OrderReturned>
    {
        private readonly IReturnRepository _returnRepository;

        public OrderReturnedHandler(IReturnRepository returnRepository)
        {
            _returnRepository = returnRepository;
        }
        public async Task HandleAsync(OrderReturned @event)
        {
            var @return = await _returnRepository.GetReturnByOrderIdAsync(@event.Order.Id);
            if(@return is not null)
            {
                throw new ReturnCreateForTheSameOrderException(@event.Order.Id);
            }
            await _returnRepository.CreateReturnAsync(new Entity.Return(
                    Guid.NewGuid(),
                    @event.Customer,
                    @event.Order,
                    @event.Products.Select(p => new ReturnProduct(p.SKU, p.Name, p.Price, p.Quantity, p.ImagePathUrl)),
                    @event.ReasonForReturn,
                    @event.CreatedAt
                ));
        }
    }
}
