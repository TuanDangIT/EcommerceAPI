using Ecommerce.Modules.Orders.Application.Returns.Events;
using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
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
        private readonly TimeProvider _timeProvider;

        public RejectReturnHandler(IReturnRepository returnRepository, IMessageBroker messageBroker, TimeProvider timeProvider)
        {
            _returnRepository = returnRepository;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
        }
        public async Task Handle(RejectReturn request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId);
            if (@return is null)
            {
                throw new ReturnNotFoundException(request.ReturnId);
            }
            @return.Reject(request.RejectReason, _timeProvider.GetUtcNow().UtcDateTime);
            await _returnRepository.UpdateAsync();
            //More logic
            await _messageBroker.PublishAsync(new ReturnRejected(@return.Id, @return.OrderId, @return.Order.Customer.UserId, @return.Order.Customer.FirstName, @return.Order.Customer.Email,
                request.RejectReason, @return.Products.Select(p => new { p.SKU, p.Name, p.Price, p.Quantity }), @return.CreatedAt));
        }
    }
}
