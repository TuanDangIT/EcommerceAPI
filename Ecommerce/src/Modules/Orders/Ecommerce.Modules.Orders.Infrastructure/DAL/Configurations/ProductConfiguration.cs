using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.SKU)
                    .HasMaxLength(16)
                    .IsRequired();
            builder.Property(p => p.Name)
                .HasMaxLength(64)
                .IsRequired();
            builder.Property(p => p.Price)
                .HasPrecision(11, 2)
                .IsRequired();
            builder.HasIndex(p => p.SKU).IsUnique();
            builder.ToTable("Products", p =>
            {
                p.HasCheckConstraint("CK_Product_Price", "\"Price\" >= 0");
                p.HasCheckConstraint("CK_Product_Quantity", "\"Quantity\" >= 0");
            });
        }
    }
}
