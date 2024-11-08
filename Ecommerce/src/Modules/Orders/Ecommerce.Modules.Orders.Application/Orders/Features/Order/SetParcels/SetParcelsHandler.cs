using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Modules.Orders.Domain.Shipping.Entities;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.SetParcels
{
    internal class SetParcelsHandler : ICommandHandler<SetParcels>
    {
        private readonly IOrderRepository _orderRepository;

        public SetParcelsHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(SetParcels request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);
            if(order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            order.SetParcels(request.Parcels.Select(p =>
            {
                var dimensions = new Dimensions(p.Length, p.Width, p.Height);
                var weight = new Weight(p.Weight);
                var parcel = new Parcel(dimensions, weight);
                return parcel;
            }));
            await _orderRepository.UpdateAsync();
        }
    }
}
