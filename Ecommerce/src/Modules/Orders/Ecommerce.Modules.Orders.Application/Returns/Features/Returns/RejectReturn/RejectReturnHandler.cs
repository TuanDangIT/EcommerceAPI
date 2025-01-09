using Ecommerce.Modules.Orders.Application.Returns.Events;
using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.RejectReturn
{
    internal class RejectReturnHandler : ICommandHandler<RejectReturn>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<RejectReturnHandler> _logger;
        private readonly IContextService _contextService;

        public RejectReturnHandler(IReturnRepository returnRepository, IMessageBroker messageBroker, ILogger<RejectReturnHandler> logger,
            IContextService contextService)
        {
            _returnRepository = returnRepository;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(RejectReturn request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId, cancellationToken,
                query => query.Include(r => r.Order).ThenInclude(o => o.Customer)) ?? 
                throw new ReturnNotFoundException(request.ReturnId);
            @return.Reject(request.RejectReason);
            await _returnRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Return: {returnId} was rejected by {@user}.", @return.Id,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            await _messageBroker.PublishAsync(new ReturnRejected(@return.Id, @return.OrderId, @return.Order.Customer.UserId, @return.Order.Customer.FirstName, @return.Order.Customer.Email,
                request.RejectReason, @return.Products.Select(p => new { p.SKU, p.Name, p.Price, p.Quantity }), @return.CreatedAt));
        }
    }
}
