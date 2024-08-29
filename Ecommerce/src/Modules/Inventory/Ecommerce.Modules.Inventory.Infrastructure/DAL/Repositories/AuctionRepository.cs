using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Repositories
{
    internal class AuctionRepository : IAuctionRepository
    {
        private readonly InventoryDbContext _dbContext;

        public AuctionRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> AddAsync(Auction auction)
        {
            await _dbContext.Auctions.AddAsync(auction);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> AddManyAsync(IEnumerable<Auction> auctions)
        {
            await _dbContext.Auctions.AddRangeAsync(auctions);  
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid auctionId)
            => await _dbContext.Auctions.Where(a => a.Id == auctionId).ExecuteDeleteAsync();

        public async Task<int> DeleteManyAsync(Guid[] auctionIds) 
            => await _dbContext.Auctions.Where(a => auctionIds.Contains(a.Id)).ExecuteDeleteAsync();

        public async Task<Auction?> GetAsync(Guid auctionId)
            => await _dbContext.Auctions.SingleOrDefaultAsync(a => a.Id == auctionId);

        public async Task<int> UpdateAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
