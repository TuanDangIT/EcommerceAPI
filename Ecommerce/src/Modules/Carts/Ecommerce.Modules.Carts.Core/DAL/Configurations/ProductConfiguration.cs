using Ecommerce.Modules.Carts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DAL.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.SKU)
                .HasMaxLength(16)
                .IsRequired();
            builder.Property(p => p.Name)
                .IsRequired();
            builder.Property(p => p.Price)
                .HasPrecision(11, 2)
                .IsRequired();
            builder.Property(p => p.ImagePathUrl)
                .IsRequired();
            builder.HasMany(p => p.CartProducts)
                .WithOne(cp => cp.Product)
                .HasForeignKey(cp => cp.ProductId);
            builder.ToTable(p =>
            {
                p.HasCheckConstraint("CK_Product_Price", "\"Price\" >= 0");
                p.HasCheckConstraint("CK_Product_Quantity", "\"Quantity\" >= 0");
            });
        }
    }
}
