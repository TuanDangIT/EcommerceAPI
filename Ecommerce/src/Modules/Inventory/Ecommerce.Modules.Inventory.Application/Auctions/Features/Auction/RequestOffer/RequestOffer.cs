using Ecommerce.Shared.Abstractions.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.RequestOffer
{
    public sealed record class RequestOffer(Guid AuctionId, decimal Price, string Reason) : ICommand;
}
