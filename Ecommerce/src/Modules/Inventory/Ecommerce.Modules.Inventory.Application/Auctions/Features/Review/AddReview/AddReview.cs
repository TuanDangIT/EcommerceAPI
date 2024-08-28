using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Review.AddReview
{
    public sealed record class AddReview : ICommand
    {
        public string Username { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Grade { get; set; }
        public Guid AuctionId { get; set; }
    }
}
