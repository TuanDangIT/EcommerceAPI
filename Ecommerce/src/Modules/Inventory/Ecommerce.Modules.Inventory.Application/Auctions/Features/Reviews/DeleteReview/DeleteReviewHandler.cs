using Ecommerce.Modules.Inventory.Application.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.DeleteReview
{
    internal class DeleteReviewHandler : ICommandHandler<DeleteReview>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<DeleteReviewHandler> _logger;
        private readonly IContextService _contextService;

        public DeleteReviewHandler(IAuctionRepository auctionRepository, IReviewRepository reviewRepository,
            ILogger<DeleteReviewHandler> logger, IContextService contextService)
        {
            _auctionRepository = auctionRepository;
            _reviewRepository = reviewRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(DeleteReview request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAsync(request.AuctionId, cancellationToken) ?? 
                throw new AuctionNotFoundException(request.AuctionId);
            var review = auction.Reviews.SingleOrDefault(r => r.Id == request.ReviewId) ?? 
                throw new ReviewNotFoundException(request.ReviewId);
            await _reviewRepository.DeleteAsync(review.Id, cancellationToken);
            _logger.LogInformation("Review: {reviewId} was deleted by {@user}.", review.Id,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
