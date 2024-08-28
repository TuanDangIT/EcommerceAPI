using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.DeleteReview
{
    public sealed record class DeleteReview(Guid AuctionId, Guid ReviewId) : ICommand;
}
