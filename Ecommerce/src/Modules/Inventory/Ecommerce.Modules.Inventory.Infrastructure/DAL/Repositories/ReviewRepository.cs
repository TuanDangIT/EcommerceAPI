using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Shared.Abstractions.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Repositories
{
    internal class ReviewRepository : IReviewRepository
    {
        private readonly InventoryDbContext _dbContext;
        public ReviewRepository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task DeleteAsync(Guid reviewId, CancellationToken cancellationToken = default)
            => await _dbContext.Reviews.Where(r => r.Id == reviewId).ExecuteDeleteAsync(cancellationToken);

        public async Task<Review?> GetAsync(Guid reviewId, CancellationToken cancellationToken = default)
            => await _dbContext.Reviews.SingleOrDefaultAsync(r => r.Id == reviewId, cancellationToken);

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
