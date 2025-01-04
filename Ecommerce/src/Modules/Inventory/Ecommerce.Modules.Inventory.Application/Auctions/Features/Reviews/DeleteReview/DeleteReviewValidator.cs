using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.DeleteReview;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Reviews.DeleteReview
{
    internal class DeleteReviewValidator : AbstractValidator<Review.DeleteReview.DeleteReview>
    {
        public DeleteReviewValidator()
        {
            RuleFor(d => d.ReviewId)
                .NotEmpty()
                .NotNull();
            RuleFor(d => d.AuctionId)
                .NotEmpty()
                .NotNull();
        }
    }
}
