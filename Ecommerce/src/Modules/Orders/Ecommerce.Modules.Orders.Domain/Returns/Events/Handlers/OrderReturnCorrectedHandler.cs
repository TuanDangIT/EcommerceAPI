using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Returns.Exception;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Events.Handlers
{
    internal class OrderReturnCorrectedHandler : IDomainEventHandler<OrderReturnCorrected>
    {
        private readonly IReturnRepository _returnRepository;

        public OrderReturnCorrectedHandler(IReturnRepository returnRepository)
        {
            _returnRepository = returnRepository;
        }

        public async Task HandleAsync(OrderReturnCorrected @event)
        {
            var @return = await _returnRepository.GetByOrderIdAsync(@event.OrderId) ?? 
                throw new ReturnNotFoundException(@event.OrderId);
            foreach(var product in @event.Products)
            {
                @return.AddProduct(new Entities.ReturnProduct(product.SKU, product.Name, product.Price, 
                    product.UnitPrice, product.Quantity, product.ImagePathUrl));
            }
            @return.ChangeStatus(ReturnStatus.Handled);
            await _returnRepository.UpdateAsync();
        }
    }
}
