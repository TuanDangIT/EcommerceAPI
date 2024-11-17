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

        public RejectReturnHandler(IReturnRepository returnRepository, IMessageBroker messageBroker)
        {
            _returnRepository = returnRepository;
            _messageBroker = messageBroker;
        }
        public async Task Handle(RejectReturn request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId);
            if (@return is null)
            {
                throw new ReturnNotFoundException(request.ReturnId);
            }
            @return.Reject(request.RejectReason);
            await _returnRepository.UpdateAsync();
            await _messageBroker.PublishAsync(new ReturnRejected(@return.Id, @return.OrderId, @return.Order.Customer.UserId, @return.Order.Customer.FirstName, @return.Order.Customer.Email,
                request.RejectReason, @return.Products.Select(p => new { p.SKU, p.Name, p.Price, p.Quantity }), @return.CreatedAt));
        }
    }
}
