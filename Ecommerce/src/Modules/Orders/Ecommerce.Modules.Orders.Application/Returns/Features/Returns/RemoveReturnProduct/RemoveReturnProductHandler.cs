using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Events;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.DomainEvents;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.RemoveReturnProduct
{
    internal class RemoveReturnProductHandler : ICommandHandler<RemoveReturnProduct>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<RemoveReturnProductHandler> _logger;
        private readonly IContextService _contextService;

        public RemoveReturnProductHandler(IReturnRepository returnRepository, IDomainEventDispatcher domainEventDispatcher,
            ILogger<RemoveReturnProductHandler> logger, IContextService contextService)
        {
            _returnRepository = returnRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(RemoveReturnProduct request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId, cancellationToken,
                query => query.Include(r => r.Products),
                query => query.Include(r => r.Order)) ??
                throw new ReturnNotFoundException(request.ReturnId);
            var product = @return.GetReturnProduct(request.ProductId) ?? 
                throw new ReturnProductNotFoundException(request.ProductId);
            @return.RemoveProduct(request.ProductId);
            await _returnRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("An return product was deleted from return: {returnId} by {@user}.",
                @return.Id, new { _contextService.Identity!.Username, _contextService.Identity.Id });
            await _domainEventDispatcher.DispatchAsync(new ReturnProductDeleted(@return.OrderId,
                product.SKU, product.Quantity));
        }
    }
}
