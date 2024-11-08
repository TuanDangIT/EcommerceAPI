﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Shared.Abstractions.Entities;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL
{
    internal class InventoryDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<ProductParameter> ProductParameters { get; set; }  
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public const string Schema = "inventory";
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<IAuditable>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = TimeProvider.System.GetUtcNow().UtcDateTime;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = TimeProvider.System.GetUtcNow().UtcDateTime;
                }
            }
            return base.SaveChangesAsync(true, cancellationToken);
        }
    }
}
