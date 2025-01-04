using Ecommerce.Modules.Inventory.Application.Auctions.Events;
using Ecommerce.Modules.Inventory.Application.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.RequestOffer
{
    internal class RequestOfferHandler : ICommandHandler<RequestOffer>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IContextService _contextService;
        private readonly ILogger<RequestOfferHandler> _logger;

        public RequestOfferHandler(IAuctionRepository auctionRepository, IMessageBroker messageBroker, IContextService contextService,
            ILogger<RequestOfferHandler> logger)
        {
            _auctionRepository = auctionRepository;
            _messageBroker = messageBroker;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task Handle(RequestOffer request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAsync(request.AuctionId, cancellationToken) ?? throw new AuctionNotFoundException(request.AuctionId);
            if(request.Price >= auction.Price)
            {
                throw new AuctionOfferPriceHigherOrEqualAuctionPrice(request.Price, auction.Price);
            }
            await _messageBroker.PublishAsync(new OfferRequested(_contextService.Identity!.Id, auction.SKU, auction.Name, request.Price, auction.Price, request.Reason));
            _logger.LogInformation("An offer: {@offer} was requested for auction: {auctionId} by {@user}.", new { request.Reason, request.Price }, request.AuctionId,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
