using Ecommerce.Modules.Orders.Application.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<WriteAdditionalInformationHandler> _logger;
        private readonly IContextService _contextService;

        public WriteAdditionalInformationHandler(IOrderRepository orderRepository, ILogger<WriteAdditionalInformationHandler> logger,
            IContextService contextService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(WriteAdditionalInformation request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken) ?? 
                throw new OrderNotFoundException(request.OrderId);
            if (request.AdditionalInformation == string.Empty)
            {
                return;
            }
            order.WriteAdditionalInformation(request.AdditionalInformation);
            await _orderRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Order's: {@order} additional information property was written by {@user}.", order,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
