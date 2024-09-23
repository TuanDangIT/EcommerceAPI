using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Modules.Orders.Application.Stripe;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Modules.Orders.Application.Returns.Events;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.HandleReturn
{
    internal sealed class HandleReturnHandler : ICommandHandler<HandleReturn>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IStripeService _stripeService;
        private readonly IMessageBroker _messageBroker;
        private readonly TimeProvider _timeProvider;

        public HandleReturnHandler(IReturnRepository returnRepository, IStripeService stripeService, IMessageBroker messageBroker, TimeProvider timeProvider)
        {
            _returnRepository = returnRepository;
            _stripeService = stripeService;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
        }
        public async Task Handle(HandleReturn request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId);
            if (@return is null)
            {
                throw new ReturnNotFoundException(request.ReturnId);
            }
            if(@return.IsFullReturn)
            {
                await _stripeService.Refund(@return.Order);
            }
            else
            {
                await _stripeService.Refund(@return.Order, @return.Products.Sum(p => p.Price));
            }
            @return.Handle(_timeProvider.GetUtcNow().UtcDateTime);
            await _returnRepository.UpdateAsync();
            await _messageBroker.PublishAsync(new ReturnHandled(@return.Products.Select(p => new { p.SKU, p.Quantity })));
        }
    }
}
