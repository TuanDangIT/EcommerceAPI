using Ecommerce.Modules.Inventory.Application.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.AddReview
{
    internal sealed class AddReviewHandler : ICommandHandler<AddReview, Guid>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IContextService _contextService;
        private readonly ILogger<AddReviewHandler> _logger;

        public AddReviewHandler(IAuctionRepository auctionRepository, IReviewRepository reviewRepository, IContextService contextService,
            ILogger<AddReviewHandler> logger)
        {
            _auctionRepository = auctionRepository;
            _reviewRepository = reviewRepository;
            _contextService = contextService;
            _logger = logger;
        }
        public async Task<Guid> Handle(AddReview request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAsync(request.AuctionId, cancellationToken, a => a.Reviews) ?? 
                throw new AuctionNotFoundException(request.AuctionId);
            var username = _contextService.Identity is not null &&
                _contextService.Identity.IsAuthenticated ?
                _contextService.Identity.Username :
                throw new UserIsNotAuthenticatedException();
            var customerId = _contextService.Identity.Id;
            var review = new Domain.Auctions.Entities.Review(
                username,
                customerId,
                request.Text,
                request.Grade);
            auction.AddReview(review);
            await _reviewRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Review: {@review} was added to auction: {auctionId} by {@user}.", new { request.Grade, request.Text },
                request.AuctionId, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            return review.Id;
        }
    }
}
