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
        public async Task AddAsync(Auction auction)
        {
            await _dbContext.Auctions.AddAsync(auction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddManyAsync(IEnumerable<Auction> auctions)
        {
            await _dbContext.Auctions.AddRangeAsync(auctions);  
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid auctionId)
            => await _dbContext.Auctions.Where(a => a.Id == auctionId).ExecuteDeleteAsync();

        public async Task DeleteManyAsync(Guid[] auctionIds) 
            => await _dbContext.Auctions.Where(a => auctionIds.Contains(a.Id)).ExecuteDeleteAsync();

        public async Task<IEnumerable<Auction>> GetAllThatContainsInArrayAsync(Guid[] auctionIds)
            => await _dbContext.Auctions
                .Where(p => auctionIds.Contains(p.Id))
                .ToListAsync();

        public async Task<Auction?> GetAsync(Guid auctionId)
            => await _dbContext.Auctions.SingleOrDefaultAsync(a => a.Id == auctionId);

        public async Task UpdateAsync()
            => await _dbContext.SaveChangesAsync();
    }
}
