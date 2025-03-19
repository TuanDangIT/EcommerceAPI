using Ecommerce.Shared.Abstractions.MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.EditReview
{
    public record class EditReview(Guid ReviewId, string Text, int Grade) : ICommand
    {
        [SwaggerIgnore]
        public Guid AuctionId { get; init; }
    }
}
