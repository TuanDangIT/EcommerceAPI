﻿using Ecommerce.Modules.Orders.Application.Returns.Exceptions;
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
using Microsoft.Extensions.Logging;
using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Return.HandleReturn
{
    internal sealed class HandleReturnHandler : ICommandHandler<HandleReturn>
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IPaymentProcessorService _stripeService;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<HandleReturnHandler> _logger;
        private readonly IContextService _contextService;

        public HandleReturnHandler(IReturnRepository returnRepository, IPaymentProcessorService stripeService, IMessageBroker messageBroker,
            ILogger<HandleReturnHandler> logger, IContextService contextService)
        {
            _returnRepository = returnRepository;
            _stripeService = stripeService;
            _messageBroker = messageBroker;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(HandleReturn request, CancellationToken cancellationToken)
        {
            var @return = await _returnRepository.GetAsync(request.ReturnId, cancellationToken,
                query => query.Include(r => r.Order).ThenInclude(o => o.Customer),
                query => query.Include(r => r.Products)) ?? 
                throw new ReturnNotFoundException(request.ReturnId);
            if (!@return.AreAllProductsAcceptedOrReturned)
            {
                throw new ReturnCannotHandleException();
            }
            //if (@return.IsFullReturn)
            //{
            //    await _stripeService.RefundAsync(@return.Order, cancellationToken);
            //}
            //else
            //{
            //    bool includeShippingPrice = !@return.HasReturned;
            //    await _stripeService.RefundAsync(@return.Order, @return.TotalSumLeftToReturn + (includeShippingPrice ? @return.Order.ShippingPrice : 0), cancellationToken);
            //}
            var productToReturn = @return.Products.Where(p => p.Status != ReturnProductStatus.Returned).ToList();
            @return.Handle();
            await _returnRepository.UpdateAsync();
            _logger.LogInformation("Return: {returnId} was handled by {@user}.", @return.Id, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            await _messageBroker.PublishAsync(new ReturnHandled(@return.Id, @return.OrderId, @return.Order.Customer!.UserId, 
                @return.Order.Customer.FirstName, @return.Order.Customer.Email, productToReturn.Select(p => new { p.Name, p.Price, p.SKU, p.Quantity }), @return.CreatedAt));
        }
    }
}
