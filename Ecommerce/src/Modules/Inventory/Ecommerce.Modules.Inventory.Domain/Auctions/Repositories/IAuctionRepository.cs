using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Auctions.Repositories
{
    public interface IAuctionRepository
    {
        Task ListManyAsync(IEnumerable<Auction> auctions);
        Task<Auction?> GetAsync(Guid auctionId);
        Task<IEnumerable<Auction>> GetAllThatContainsInArrayAsync(Guid[] auctionIds);
        Task UnlistManyAsync(Guid[] auctionIds);
        Task UpdateAsync();
    }
}
