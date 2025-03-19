using Ecommerce.Shared.Abstractions.MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.AddReview
{
    public sealed record class AddReview : ICommand
    {
        public string Text { get; init; } = string.Empty;
        public int Grade { get; init; }
        [SwaggerIgnore]
        public Guid AuctionId { get; init; }
    }
}
