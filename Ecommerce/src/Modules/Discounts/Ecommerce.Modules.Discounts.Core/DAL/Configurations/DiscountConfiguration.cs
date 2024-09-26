using Ecommerce.Modules.Discounts.Core.Entities;
using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DAL.Configurations
{
    internal class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.Property(d => d.Code)
                .HasMaxLength(10)
                .IsRequired();
            builder.HasIndex(d => d.Code)
                .IsUnique();
            builder.Property(d => d.CreatedAt) 
                .IsRequired();
            builder.Property(d => d.Type)
                .HasConversion<string>();
            builder.HasDiscriminator(d => d.Type)
                .HasValue<NominalDiscount>(DiscountType.NominalDiscount)
                .HasValue<PercentageDiscount>(DiscountType.PercentageDiscount);
        }
    }
}
