using Ecommerce.Modules.Auctions.Core.DTO;
using Ecommerce.Modules.Auctions.Core.Entities;
using Ecommerce.Shared.Infrastructure.Pagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Services
{
    public interface IAuctionService
    {
        Task<PagedResult<AuctionBrowseDto>> BrowseAsync(SieveModel sieveModel);
        Task<AuctionDetailsDto?> GetAsync(Guid auctionId);
    }
}
