using Ecommerce.Modules.Inventory.Application.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Shared.Abstractions.MediatR;
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

        public DeleteReviewHandler(IAuctionRepository auctionRepository, IReviewRepository reviewRepository)
        {
            _auctionRepository = auctionRepository;
            _reviewRepository = reviewRepository;
        }
        public async Task Handle(DeleteReview request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAsync(request.AuctionId);
            if(auction is null)
            {
                throw new AuctionNotFoundException(request.AuctionId);
            }
            var review = await _reviewRepository.GetAsync(request.ReviewId);
            if(review is null)
            {
                throw new ReviewNotFoundException(request.ReviewId);
            }
            var rowsChanged = await _reviewRepository.DeleteAsync(request.ReviewId);
            if(rowsChanged != 1)
            {
                throw new ReviewNotDeletedException();
            }
        }
    }
}
