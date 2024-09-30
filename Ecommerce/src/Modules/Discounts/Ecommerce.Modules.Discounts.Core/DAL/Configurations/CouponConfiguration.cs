using Ecommerce.Modules.Discounts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DAL.Configurations
{
    internal class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.Property(c => c.Name)
                .HasMaxLength(36)
                .IsRequired();
            builder.Property(c => c.Type)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(c => c.StripeCouponId)
                .IsRequired();
            builder.Property(c => c.CreatedAt)
                .IsRequired();
            builder.HasDiscriminator(c => c.Type)
                .HasValue<NominalCoupon>(Entities.Enums.CouponType.NominalCoupon)
                .HasValue<PercentageCoupon>(Entities.Enums.CouponType.PercentageCoupon);
            builder.HasMany(c => c.Discounts)
                .WithOne(d => d.Coupon)
                .HasForeignKey(d => d.CouponId);
            builder.Property(c => c.StripeCouponId)
                .IsRequired();
            builder.HasIndex(c => c.StripeCouponId)
                .IsUnique();
        }
    }
}
