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
        Task<Auction?> GetAsync(Guid auctionId);
        Task<int> DeleteAsync(Guid auctionId);
        Task<int> UpdateAsync();
    }
}
