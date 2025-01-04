using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EditCustomerHandler> _logger;
        private readonly IContextService _contextService;

        public EditCustomerHandler(IOrderRepository orderRepository, ILogger<EditCustomerHandler> logger, IContextService contextService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(EditCustomer request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken) ?? 
                throw new OrderNotFoundException(request.OrderId);
            var address = new Address(request.Address.Street, request.Address.BuildingNumber, request.Address.City, request.Address.PostCode);
            order.EditCustomer(new Customer(request.FirstName, request.LastName, request.Email, request.PhoneNumber, address));
            await _orderRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Order's: {orderId} customer was editted with new details {@updatingDetails} by {@user}.", 
                order.Id,
                new { request.FirstName, request.LastName, request.Email, request.PhoneNumber, address }, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
