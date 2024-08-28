using Ecommerce.Modules.Auctions.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.DAL.Configurations
{
    internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(r => r.Username).IsRequired();
            builder.Property(r => r.Text).IsRequired();
            builder.Property(r => r.Grade).IsRequired();
            builder.Property(r => r.AuctionId).IsRequired();
            builder.Property(r => r.CreatedAt).IsRequired();
        }
    }
}
