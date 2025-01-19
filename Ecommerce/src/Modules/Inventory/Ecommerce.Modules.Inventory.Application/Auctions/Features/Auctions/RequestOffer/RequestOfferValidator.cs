using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.RequestOffer
{
    internal class RequestOfferValidator : AbstractValidator<RequestOffer>
    {
        public RequestOfferValidator()
        {
            RuleFor(ro => ro.AuctionId)
                .NotEmpty()
                .NotNull();
            RuleFor(ro => ro.Price)
                .PrecisionScale(11, 2, true)
                .GreaterThanOrEqualTo(0)
                .NotEmpty();
            RuleFor(ro => ro.Reason)
                .MaximumLength(256)
                .NotEmpty()
                .NotNull();
        }
    }
}
