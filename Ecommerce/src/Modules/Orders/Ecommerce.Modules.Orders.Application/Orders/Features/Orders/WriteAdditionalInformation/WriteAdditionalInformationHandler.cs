using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.WriteAdditionalInformation
{
    internal class WriteAdditionalInformationHandler : ICommandHandler<WriteAdditionalInformation>
    {
        private readonly IOrderRepository _orderRepository;

        public WriteAdditionalInformationHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(WriteAdditionalInformation request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            if(request.AdditionalInformation == string.Empty)
            {
                return;
            }
            order.WriteAdditionalInformation(request.AdditionalInformation);
            await _orderRepository.UpdateAsync();
        }
    }
}
