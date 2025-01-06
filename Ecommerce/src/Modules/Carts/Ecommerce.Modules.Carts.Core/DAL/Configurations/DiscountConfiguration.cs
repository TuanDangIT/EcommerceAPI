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
    internal class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.Property(d => d.Code)
                .HasMaxLength(48)
                .IsRequired();
            builder.HasIndex(d => d.Code)
                .IsUnique();
            builder.Property(d => d.Type)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(d => d.Value)
                .IsRequired();
            builder.HasMany(d => d.CheckoutCarts)
                .WithOne(cc => cc.Discount)
                .HasForeignKey(cc => cc.DiscountId);
            builder.HasMany(d => d.Carts)
                .WithOne(cc => cc.Discount)
                .HasForeignKey(cc => cc.DiscountId);
            builder.Property(d => d.SKU)
                .HasMaxLength(16);
            builder.ToTable(d =>
            {
                d.HasCheckConstraint("CK_Discount_Value", "\"Value\" >= 0");
                d.HasCheckConstraint("CK_Discount_ExpiresAt", "\"ExpiresAt\" > NOW()");
            });
        }
    }
}
