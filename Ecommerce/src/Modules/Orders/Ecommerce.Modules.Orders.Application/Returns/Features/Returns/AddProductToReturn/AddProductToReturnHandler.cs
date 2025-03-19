using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Events;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.AddProductToReturn
{
    internal class AddProductToReturnHandler : ICommandHandler<AddProductToReturn>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<AddProductToReturnHandler> _logger;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public AddProductToReturnHandler(IReturnRepository returnRepository, IContextService contextService, ILogger<AddProductToReturnHandler> logger,
            IDomainEventDispatcher domainEventDispatcher)
        {
            _returnRepository = returnRepository;
            _contextService = contextService;
            _logger = logger;
            _domainEventDispatcher = domainEventDispatcher;
        }
        public async Task Handle(AddProductToReturn request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId, cancellationToken,
                query => query.Include(r => r.Order).ThenInclude(o => o.Products),
                query => query.Include(r => r.Products)) ??
                throw new ReturnNotFoundException(request.ReturnId);
            @return.AddProduct(request.SKU, request.Quantity);
            await _returnRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Product: {sku} was added to return: {returnId} by {@user}.", request.SKU, request.ReturnId,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            await _domainEventDispatcher.DispatchAsync(new ReturnProductAdded(@return.OrderId, request.SKU, request.Quantity));
        }
    }
}
