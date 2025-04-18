﻿using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public async Task ListManyAsync(IEnumerable<Auction> auctions, CancellationToken cancellationToken = default)
        {
            await _dbContext.Auctions.AddRangeAsync(auctions, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UnlistManyAsync(Guid[] auctionIds, CancellationToken cancellationToken = default) 
            => await _dbContext.Auctions.Where(a => auctionIds.Contains(a.Id)).ExecuteDeleteAsync(cancellationToken);

        public async Task<IEnumerable<Auction>> GetAllThatContainsInArrayAsync(Guid[] auctionIds, CancellationToken cancellationToken = default)
            => await _dbContext.Auctions
                .Where(p => auctionIds.Contains(p.Id))
                .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Auction>> GetAllThatContainsInArrayAsync(string[] skus, CancellationToken cancellationToken = default)
            => await _dbContext.Auctions
                .Where(p => skus.Contains(p.SKU))
                .ToListAsync(cancellationToken);

        public async Task<Auction?> GetAsync(Guid auctionId, CancellationToken cancellationToken = default,
            params Expression<Func<Auction, object>>[] includes)
        {
            var query = _dbContext.Auctions
                .AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var auction = await query
                .FirstOrDefaultAsync(a => a.Id == auctionId, cancellationToken);
            return auction;
        }

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);

    }
}
