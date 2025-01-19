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

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.DeleteReturn
{
    internal class DeleteReturnHandler : ICommandHandler<DeleteReturn>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<DeleteReturn> _logger;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public DeleteReturnHandler(IReturnRepository returnRepository, IContextService contextService,
            ILogger<DeleteReturn> logger, IDomainEventDispatcher domainEventDispatcher)
        {
            _returnRepository = returnRepository;
            _contextService = contextService;
            _logger = logger;
            _domainEventDispatcher = domainEventDispatcher;
        }
        public async Task Handle(DeleteReturn request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId, cancellationToken,
                query => query.Include(r => r.Order),
                query => query.Include(r => r.Products)) ??
                throw new ReturnNotFoundException(request.ReturnId);
            await _returnRepository.DeleteAsync(request.ReturnId, cancellationToken);
            _logger.LogInformation("Return: {returnId} was deleted by {@user}.", request.ReturnId,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            await _domainEventDispatcher.DispatchAsync(new ReturnDeleted(@return.OrderId, @return.Products));
        }
    }
}
