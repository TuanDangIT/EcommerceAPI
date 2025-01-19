using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;
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

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.SetReturnProductQuantity
{
    internal class SetReturnProductQuantityHandler : ICommandHandler<SetReturnProductQuantity>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<SetReturnProductQuantityHandler> _logger;
        private readonly IContextService _contextService;

        public SetReturnProductQuantityHandler(IReturnRepository returnRepository, IDomainEventDispatcher domainEventDispatcher,
            ILogger<SetReturnProductQuantityHandler> logger, IContextService contextService)
        {
            _returnRepository = returnRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(SetReturnProductQuantity request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId, cancellationToken,
                query => query.Include(r => r.Products),
                query => query.Include(r => r.Order)) ??
                throw new ReturnNotFoundException(request.ReturnId);
            var product = @return.GetReturnProduct(request.ProductId) ??
                throw new ReturnProductNotFoundException(request.ProductId);
            if(product.Quantity == request.Quantity)
            {
                return;
            }
            var oldQuantity = product.Quantity;
            @return.SetProductQuantity(request.ProductId, request.Quantity);
            if (!@return.HasProducts)
            {
                await _returnRepository.DeleteAsync(@return.Id, cancellationToken);
                _logger.LogInformation("Product was deleted and the return: {returnId} was empty therefore deleted by {@user}.",
                    @return.Id, new { _contextService.Identity!.Username, _contextService.Identity.Id });
                await _domainEventDispatcher.DispatchAsync(new ReturnDeleted(@return.OrderId, [new ReturnProduct(product.SKU, oldQuantity)]));
                return;
            }
            product.SetStatus(ReturnProductStatus.Unknown);
            await _returnRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Return's: {returnId} product: {productId} was set to new value: {newQuantity} by {@user}.",
                @return.Id, request.ProductId, request.Quantity, new { _contextService.Identity!.Username, _contextService.Identity.Id });
            await _domainEventDispatcher.DispatchAsync(new ReturnProductQuantitySet(@return.OrderId,
                product.SKU, oldQuantity - request.Quantity));
        }
    }
}
