using Ecommerce.Modules.Auctions.Core.DTO;
using Ecommerce.Shared.Infrastructure.Pagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Services
{
    public interface IReviewService
    {
        Task<PagedResult<ReviewBrowseDto>> BrowseAsync(SieveModel sieveModel, Guid auctionId);
        Task<int> DeleteAsync(Guid reviewId);
        Task<int> UpdateAsync(ReviewUpdateDto updateReviewDto, Guid auctionId);
        Task<int> AddAsync(ReviewAddDto reviewAddDto, Guid auctionId);
    }
}
