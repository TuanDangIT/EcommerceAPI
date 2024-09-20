using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.SubmitComplaint
{
    internal sealed class SubmitComplaintHandler : ICommandHandler<SubmitComplaint>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly TimeProvider _timeProvider;

        public SubmitComplaintHandler(IOrderRepository orderRepository, IDomainEventDispatcher domainEventDispatcher, TimeProvider timeProvider)
        {
            _orderRepository = orderRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _timeProvider = timeProvider;
        }
        public async Task Handle(SubmitComplaint request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderAsync(request.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            await _domainEventDispatcher.DispatchAsync(new ComplaintSubmitted(
                    request.Title,
                    request.Description,
                    order.Customer,
                    order,
                    _timeProvider.GetUtcNow().UtcDateTime
                ));
        }
    }
}
