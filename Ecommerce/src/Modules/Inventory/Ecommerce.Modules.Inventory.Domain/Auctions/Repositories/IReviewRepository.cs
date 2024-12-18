using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Shared.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Repositories
{
    public interface IReviewRepository
    {
        //Task<PagedResult<ReviewBrowseDto>> BrowseAsync(SieveModel sieveModel, Guid auctionId);
        Task<Review?> GetAsync(Guid reviewId, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid reviewId, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
        //Task<int> AddAsync(Review review, Guid auctionId);
    }
}
