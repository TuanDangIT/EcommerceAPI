using Ecommerce.Modules.Inventory.Application.Auctions.Events;
using Ecommerce.Modules.Inventory.Application.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Abstractions.Messaging;
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

        public RequestOfferHandler(IAuctionRepository auctionRepository, IMessageBroker messageBroker, IContextService contextService)
        {
            _auctionRepository = auctionRepository;
            _messageBroker = messageBroker;
            _contextService = contextService;
        }
        public async Task Handle(RequestOffer request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAsync(request.AuctionId) ?? throw new AuctionNotFoundException(request.AuctionId);
            if(request.Price >= auction.Price)
            {
                throw new AuctionOfferPriceHigherOrEqualAuctionPrice(request.Price, auction.Price);
            }
            await _messageBroker.PublishAsync(new OfferRequested(_contextService.Identity!.Id, auction.SKU, auction.Name, request.Price, auction.Price, request.Reason));
        }
    }
}
