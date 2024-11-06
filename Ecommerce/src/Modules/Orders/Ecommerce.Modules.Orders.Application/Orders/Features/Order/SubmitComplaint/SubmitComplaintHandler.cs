using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Domain.Orders.Events;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
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
        private readonly IOrdersEventMapper _ordersEventMapper;
        private readonly IMessageBroker _messageBroker;
        private readonly TimeProvider _timeProvider;

        public SubmitComplaintHandler(IOrderRepository orderRepository, IDomainEventDispatcher domainEventDispatcher, IOrdersEventMapper ordersEventMapper, 
            IMessageBroker messageBroker, TimeProvider timeProvider)
        {
            _orderRepository = orderRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _ordersEventMapper = ordersEventMapper;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
        }
        public async Task Handle(SubmitComplaint request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            var domainEvent = new ComplaintSubmitted(
                    order.Id,
                    order.Customer.UserId,
                    order.Customer.FirstName,
                    order.Customer.Email,
                    request.Title,
                    request.Description,
                    _timeProvider.GetUtcNow().UtcDateTime
                );
            await _domainEventDispatcher.DispatchAsync(domainEvent);
            var integrationEvent = _ordersEventMapper.Map(domainEvent);
            await _messageBroker.PublishAsync(integrationEvent);
        }
    }
}
