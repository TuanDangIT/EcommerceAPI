using Ecommerce.Modules.Inventory.Application.Auctions.DTO;
using Ecommerce.Shared.Abstractions.MediatR;
using Ecommerce.Shared.Infrastructure.Pagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Features.Auction.BrowseAuctions
{
    public sealed class BrowseAuctions : SieveModel, IQuery<PagedResult<AuctionBrowseDto>>;
}
