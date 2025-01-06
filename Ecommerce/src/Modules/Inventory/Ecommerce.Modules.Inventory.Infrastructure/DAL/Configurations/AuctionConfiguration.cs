using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.DAL.Configurations
{
    internal class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.Property(p => p.SKU).IsRequired();
            //builder.HasIndex(p => p.SKU).IsUnique();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.Category).IsRequired();
            builder.OwnsMany(p => p.Parameters);
            builder.Property(p => p.Version)
                .IsConcurrencyToken();
            builder.ToTable(p =>
            {
                p.HasCheckConstraint("CK_Auction_Price", "\"Price\" >= 0");
                p.HasCheckConstraint("CK_Auction_Quantity", "\"Quantity\" > 0");
            });
        }
    }
}
