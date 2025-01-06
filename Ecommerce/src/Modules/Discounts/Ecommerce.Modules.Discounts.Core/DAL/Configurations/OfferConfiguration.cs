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
    internal class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.Property(o => o.OfferedPrice)
                .HasPrecision(11, 2)
                .IsRequired();
            builder.Property(o => o.Status)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(o => o.Reason)
                .HasMaxLength(256)
                .IsRequired();
            builder.Property(o => o.CreatedAt)
                .IsRequired();
            builder.ToTable(o =>
            {
                o.HasCheckConstraint("CK_Offer_ExpiresAt", "\"ExpiresAt\" > NOW()");
                o.HasCheckConstraint("CK_Offer_OldPrice", "\"OldPrice\" >= 0");
                o.HasCheckConstraint("CK_Offer_OfferedPrice", "\"OfferedPrice\" >= 0");
                o.HasCheckConstraint("CK_Offer_PriceComparison", "\"OldPrice\" > \"OfferedPrice\"");
            });
        }
    }
}
