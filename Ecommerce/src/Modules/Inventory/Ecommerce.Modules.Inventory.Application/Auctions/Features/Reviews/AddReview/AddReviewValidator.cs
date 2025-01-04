using Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.AddReview;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Reviews.AddReview
{
    internal class AddReviewValidator : AbstractValidator<Review.AddReview.AddReview>
    {
        public AddReviewValidator()
        {
            RuleFor(a => a.Username)
                .NotEmpty()
                .NotNull();
            RuleFor(a => a.Text)
                .NotEmpty()
                .NotNull();
            RuleFor(a => a.Grade)
                .NotEmpty()
                .NotNull()
                .InclusiveBetween(0, 10)
                .WithMessage("Grade must be between 0 and 10.");
            RuleFor(a => a.AuctionId)
                .NotEmpty()
                .NotNull();
        }
    }
}
