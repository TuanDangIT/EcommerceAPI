using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.SetTotalPaidSum
{
    internal class SetTotalPaidSumHandler : ICommandHandler<SetTotalPaidSum>
    {
        private readonly IOrderRepository _orderRepository;

        public SetTotalPaidSumHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(SetTotalPaidSum request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken) ?? throw new OrderNotFoundException(request.OrderId);

            order.SetTotalPaidSum(request.PaidSum);

            await _orderRepository.UpdateAsync(cancellationToken);
        }
    }
}
