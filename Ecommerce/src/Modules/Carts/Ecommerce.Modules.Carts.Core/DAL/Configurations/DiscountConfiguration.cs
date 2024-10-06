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
        }
    }
}
