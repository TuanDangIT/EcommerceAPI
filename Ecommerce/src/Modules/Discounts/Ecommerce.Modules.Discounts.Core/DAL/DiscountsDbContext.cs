using Ecommerce.Modules.Discounts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DAL
{
    public class DiscountsDbContext : DbContext, IDiscountDbContext
    {
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public const string Schema = "discounts";
        public DiscountsDbContext(DbContextOptions<DiscountsDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async Task<int> SaveChangesAsync()
            => await base.SaveChangesAsync();
    }
}
