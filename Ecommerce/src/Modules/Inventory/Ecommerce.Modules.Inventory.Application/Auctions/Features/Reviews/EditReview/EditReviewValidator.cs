using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.EditReview;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Reviews.EditReview
{
    internal class EditReviewValidator : AbstractValidator<Review.EditReview.EditReview>
    {
        public EditReviewValidator()
        {
            RuleFor(e => e.ReviewId)
                .NotEmpty()
                .NotNull();
            RuleFor(e => e.AuctionId)
                .NotEmpty()
                .NotNull();
            RuleFor(e => e.Text)
                .NotEmpty()
                .NotNull();
            RuleFor(e => e.Grade)
                .NotEmpty()
                .NotNull()
                .InclusiveBetween(0, 10)
                .WithMessage("Grade must be between 0 and 10.");
        }
    }
}
