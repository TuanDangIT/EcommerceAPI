﻿using Ecommerce.Modules.Discounts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DAL.Configurations
{
    internal class NominalCouponConfiguration : IEntityTypeConfiguration<NominalCoupon>
    {
        public void Configure(EntityTypeBuilder<NominalCoupon> builder)
        {
            builder.Property(nd => nd.NominalValue)
                .HasPrecision(11, 2)
                .IsRequired();
            builder.ToTable(nd =>
            {
                nd.HasCheckConstraint("CK_Coupon_NominalValue", "\"NominalValue\" >= 0");
            });
        }
    }
}
