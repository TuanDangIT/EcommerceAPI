using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Ecommerce.Modules.Carts.Tests.Unit")]
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

        private const string _schema = "carts";
        private readonly TimeProvider _timeProvider;

        public CartsDbContext(DbContextOptions<CartsDbContext> options, TimeProvider timeProvider) : base(options)
        {
            _timeProvider = timeProvider;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasDefaultSchema(_schema);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IAuditable>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = _timeProvider.GetUtcNow().UtcDateTime;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = _timeProvider.GetUtcNow().UtcDateTime;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => Database.BeginTransactionAsync(cancellationToken);
    }
}
