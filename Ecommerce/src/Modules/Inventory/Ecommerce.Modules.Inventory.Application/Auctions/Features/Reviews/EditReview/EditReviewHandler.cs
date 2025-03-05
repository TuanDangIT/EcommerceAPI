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

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.EditReview
{
    internal sealed class EditReviewHandler : ICommandHandler<EditReview>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<EditReviewHandler> _logger;
        private readonly IContextService _contextService;

        public EditReviewHandler(IAuctionRepository auctionRepository, IReviewRepository reviewRepository,
            ILogger<EditReviewHandler> logger, IContextService contextService)
        {
            _auctionRepository = auctionRepository;
            _reviewRepository = reviewRepository;
            _logger = logger;
            _contextService = contextService;
        }
        public async Task Handle(EditReview request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAsync(request.AuctionId, cancellationToken, a => a.Reviews) ?? 
                throw new AuctionNotFoundException(request.AuctionId);
            var review = auction.Reviews.FirstOrDefault(r => r.Id == request.ReviewId) ?? 
                throw new ReviewNotFoundException(request.ReviewId);
            review.Edit(request.Text, request.Grade);
            await _reviewRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Review: {reviewId} was edited by {@user}.", request.ReviewId,
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}
