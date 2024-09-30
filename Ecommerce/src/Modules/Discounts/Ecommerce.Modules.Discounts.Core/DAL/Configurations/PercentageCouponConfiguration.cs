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
    internal class PercentageCouponConfiguration : IEntityTypeConfiguration<PercentageCoupon>
    {
        public void Configure(EntityTypeBuilder<PercentageCoupon> builder)
        {
            builder.Property(pd => pd.Percent)
                .HasPrecision(2, 2)
                .IsRequired();
        }
    }
}
