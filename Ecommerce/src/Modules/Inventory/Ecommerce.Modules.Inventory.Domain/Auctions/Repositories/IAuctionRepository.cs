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
        Task<int> AddAsync(Auction auction);
        Task<int> AddManyAsync(IEnumerable<Auction> auctions);
        Task<Auction?> GetAsync(Guid auctionId);
        Task<IEnumerable<Auction>> GetAllThatContainsInArrayAsync(Guid[] auctionIds);
        Task<int> DeleteAsync(Guid auctionId);
        Task<int> DeleteManyAsync(Guid[] auctionIds);
        Task<int> UpdateAsync();
    }
}
