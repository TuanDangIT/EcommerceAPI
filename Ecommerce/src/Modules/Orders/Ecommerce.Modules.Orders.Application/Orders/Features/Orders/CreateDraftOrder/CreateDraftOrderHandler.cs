using Ecommerce.Modules.Orders.Domain.Orders.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Orders.CreateDraftOrder
{
    internal class CreateDraftOrderHandler : ICommandHandler<CreateDraftOrder, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CreateDraftOrderHandler> _logger;
        private readonly IContextService _contextService;

        public CreateDraftOrderHandler(IOrderRepository orderRepository, ILogger<CreateDraftOrderHandler> logger,
            IContextService contextService)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task<Guid> Handle(CreateDraftOrder request, CancellationToken cancellationToken)
        {
            var newGuid = Guid.NewGuid();
            var order = Domain.Orders.Entities.Order.CreateDraft(newGuid);
            await _orderRepository.CreateAsync(order, cancellationToken);
            _logger.LogInformation("Order draft was created by {@user}.", new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            return newGuid; 
        }
    }
}
