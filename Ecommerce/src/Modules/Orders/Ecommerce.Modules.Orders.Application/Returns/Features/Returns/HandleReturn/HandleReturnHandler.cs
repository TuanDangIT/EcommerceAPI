using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Modules.Orders.Application.Returns.Events;
using Ecommerce.Modules.Orders.Application.Shared.Stripe;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.HandleReturn
{
    internal sealed class HandleReturnHandler : ICommandHandler<HandleReturn>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IStripeService _stripeService;
        private readonly IMessageBroker _messageBroker;

        public HandleReturnHandler(IReturnRepository returnRepository, IStripeService stripeService, IMessageBroker messageBroker)
        {
            _returnRepository = returnRepository;
            _stripeService = stripeService;
            _messageBroker = messageBroker;
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
                await _stripeService.Refund(@return.Order, @return.Products.Sum(p => p.Price*p.Quantity));
            }
            @return.Handle();
            await _returnRepository.UpdateAsync();
            await _messageBroker.PublishAsync(new ReturnHandled(@return.Id, @return.OrderId, @return.Order.Customer.UserId, @return.Order.Customer.FirstName, @return.Order.Customer.Email,
                @return.Products.Select(p => new { p.SKU, p.Quantity }), @return.CreatedAt));
        }
    }
}
