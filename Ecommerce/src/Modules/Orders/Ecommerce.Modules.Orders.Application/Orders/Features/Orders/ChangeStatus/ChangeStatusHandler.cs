using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.ChangeStatus
{
    internal class ChangeStatusHandler : ICommandHandler<ChangeStatus>
    {
        private readonly IOrderRepository _orderRepository;

        public ChangeStatusHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(ChangeStatus request, CancellationToken cancellationToken)
        {
            if(Enum.TryParse(typeof(OrderStatus), request.Status, true, out var status))
            {
                throw new OrderInvalidStatusException(request.Status);
            }
            var order = await _orderRepository.GetAsync(request.OrderId);
            if(order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            order.ChangeStatus((OrderStatus)status!);
            await _orderRepository.UpdateAsync();
        }
    }
}
