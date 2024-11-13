using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DAL
{
    internal class CartsDbContext : DbContext, ICartsDbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CheckoutCart> CheckoutCarts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        DatabaseFacade ICartsDbContext.Database { get => base.Database; }

        public const string Schema = "carts";
        public CartsDbContext(DbContextOptions<CartsDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasDefaultSchema(Schema);
        }
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        //public Task<IDbContextTransaction> BeginTransactionAsync()
        //{
        //    return base.Database.BeginTransactionAsync();
        //}
    }
}
