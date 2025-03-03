using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
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
            builder.Property(p => p.SKU)
                .HasMaxLength(16)
                .IsRequired();
            builder.HasIndex(p => p.SKU).IsUnique();
            builder.Property(p => p.EAN)
                .HasMaxLength(13);
            builder.Property(p => p.Name)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(p => p.Price)
                .HasPrecision(11, 2)
                .IsRequired();
            builder.Property(p => p.VAT)
                .HasMaxLength(3);
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.Location)
                .HasMaxLength(64);
            builder.HasMany(p => p.Parameters)
                .WithMany(p => p.Products)
                .UsingEntity<ProductParameter>(
                    p => p.HasOne(pp => pp.Parameter)
                        .WithMany(pp => pp.ProductParameters)
                        .HasForeignKey(pp => pp.ParameterId),
                    p => p.HasOne(pp => pp.Product)
                        .WithMany(pp => pp.ProductParameters)
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
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(p => p.Manufacturer)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ManufacturerId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(p => p.Version)
                .IsConcurrencyToken();
            builder.ToTable(p =>
            {
                p.HasCheckConstraint("CK_Product_Price", "\"Price\" >= 0");
                p.HasCheckConstraint("CK_Product_Quantity", "\"Quantity\" >= 0");
                p.HasCheckConstraint("CK_Product_VAT", "\"VAT\" >= 0");
                p.HasCheckConstraint("CK_Product_Reserved", "\"Reserved\" >= 0");
            });
        }
    }
}
