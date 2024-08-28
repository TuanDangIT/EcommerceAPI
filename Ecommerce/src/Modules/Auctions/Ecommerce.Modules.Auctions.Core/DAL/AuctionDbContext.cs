using Ecommerce.Modules.Auctions.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.DAL
{
    internal class AuctionDbContext : DbContext, IAuctionDbContext
    {
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("auction");
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
