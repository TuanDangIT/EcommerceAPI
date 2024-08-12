using Ecommerce.Modules.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.SKU).IsRequired();
            builder.HasIndex(p => p.SKU).IsUnique();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.HasMany(p => p.Parameters)
                .WithMany(p => p.Products)
                .UsingEntity<ProductParameter>(
                    p => p.HasOne(pp => pp.Parameter)
                        .WithMany()
                        .HasForeignKey(pp => pp.ParameterId),
                    p => p.HasOne(pp => pp.Product)
                        .WithMany()
                        .HasForeignKey(pp => pp.ProductId),
                    pp =>
                    {
                        pp.Property(pp => pp.Value).IsRequired();
                    }
                );
            builder.HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
