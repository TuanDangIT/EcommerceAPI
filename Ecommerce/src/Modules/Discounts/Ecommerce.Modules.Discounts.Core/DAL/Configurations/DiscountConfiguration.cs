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
                .HasMaxLength(48)
                .IsRequired();
            builder.HasIndex(d => d.Code)
                .IsUnique();
            builder.Property(d => d.IsActive)
                .IsRequired();
            builder.Property(d => d.Redemptions)
                .IsRequired();
            builder.Property(d => d.CreatedAt) 
                .IsRequired();
            builder.Property(d => d.StripePromotionCodeId)
                .IsRequired();
            builder.Property(d => d.CouponId)
                .IsRequired();
            builder.ToTable(d =>
            {
                d.HasCheckConstraint("CK_Discount_Redemptions", "\"Redemptions\" >= 0");
                d.HasCheckConstraint("CK_Discount_ExpiresAt", "\"ExpiresAt\" > NOW()");
            });
            //builder.Property(d => d.Coupon)
            //    .IsRequired();
            //builder.Property(d => d.Type)
            //    .HasConversion<string>()
            //    .IsRequired();
            //builder.HasDiscriminator(d => d.Type)
            //    .HasValue<NominalDiscount>(DiscountType.NominalDiscount)
            //    .HasValue<PercentageDiscount>(DiscountType.PercentageDiscount);
        }
    }
}
