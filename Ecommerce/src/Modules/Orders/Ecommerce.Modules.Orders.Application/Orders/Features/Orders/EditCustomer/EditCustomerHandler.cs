using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.EditCustomer
{
    internal class EditCustomerHandler : ICommandHandler<EditCustomer>
    {
        private readonly IOrderRepository _orderRepository;

        public EditCustomerHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(EditCustomer request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId);
            if(order is null)
            {
                throw new OrderNotFoundException(request.OrderId);
            }
            var address = new Address(request.Address.Street, request.Address.BuildingNumber, request.Address.City, request.Address.PostCode);
            order.EditCustomer(new Customer(request.FirstName, request.LastName, request.Email, request.PhoneNumber, address));
            await _orderRepository.UpdateAsync();
        }
    }
}
