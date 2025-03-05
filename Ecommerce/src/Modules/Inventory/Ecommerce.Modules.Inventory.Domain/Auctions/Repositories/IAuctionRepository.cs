using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Repositories
{
    public interface IAuctionRepository
    {
        Task ListManyAsync(IEnumerable<Auction> auctions, CancellationToken cancellationToken = default);
        Task<Auction?> GetAsync(Guid auctionId, CancellationToken cancellationToken = default,
            params Expression<Func<Auction, object>>[] includes);
        Task<IEnumerable<Auction>> GetAllThatContainsInArrayAsync(Guid[] auctionIds, CancellationToken cancellationToken = default);
        Task UnlistManyAsync(Guid[] auctionIds, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
    }
}
