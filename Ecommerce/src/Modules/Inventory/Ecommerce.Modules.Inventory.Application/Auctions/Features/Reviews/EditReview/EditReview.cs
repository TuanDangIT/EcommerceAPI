using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.EditReview
{
    public record class EditReview(Guid AuctionId, Guid ReviewId, string Text, int Grade) : ICommand;
}
