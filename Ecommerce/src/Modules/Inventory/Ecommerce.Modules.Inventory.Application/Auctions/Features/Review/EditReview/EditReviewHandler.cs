using Ecommerce.Modules.Inventory.Application.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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
        private readonly TimeProvider _timeProvider;

        public EditReviewHandler(IAuctionRepository auctionRepository, IReviewRepository reviewRepository, TimeProvider timeProvider)
        {
            _auctionRepository = auctionRepository;
            _reviewRepository = reviewRepository;
            _timeProvider = timeProvider;
        }
        public async Task Handle(EditReview request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAsync(request.AuctionId);
            if (auction is null)
            {
                throw new AuctionNotFoundException(request.AuctionId);
            }
            var review = await _reviewRepository.GetAsync(request.ReviewId);
            if (review is null)
            {
                throw new ReviewNotFoundException(request.ReviewId);
            }
            review.EditReview(request.Text, request.Grade, _timeProvider.GetUtcNow().UtcDateTime);
            await _reviewRepository.UpdateAsync();
        }
    }
}
