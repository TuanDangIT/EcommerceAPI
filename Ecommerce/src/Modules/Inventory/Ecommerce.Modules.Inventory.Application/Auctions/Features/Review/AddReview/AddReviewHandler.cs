using Ecommerce.Modules.Inventory.Application.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.AddReview
{
    internal sealed class AddReviewHandler : ICommandHandler<AddReview>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IContextService _contextService;

        public AddReviewHandler(IAuctionRepository auctionRepository, IReviewRepository reviewRepository, IContextService contextService)
        {
            _auctionRepository = auctionRepository;
            _reviewRepository = reviewRepository;
            _contextService = contextService;
        }
        public async Task Handle(AddReview request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAsync(request.AuctionId);
            if(auction is null)
            {
                throw new AuctionNotFoundException(request.AuctionId);
            }
            var username = _contextService.Identity is not null &&
                _contextService.Identity.IsAuthenticated ?
                _contextService.Identity.Username :
                throw new UserIsNotAuthenticatedException();
            auction.AddReview(new Domain.Auctions.Entities.Review(
                Guid.NewGuid(),
                username,
                request.Text,
                request.Grade));
            await _reviewRepository.UpdateAsync();
        }
    }
}
