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
    internal class NominalDiscountConfiguration : IEntityTypeConfiguration<NominalDiscount>
    {
        public void Configure(EntityTypeBuilder<NominalDiscount> builder)
        {
            builder.Property(nd => nd.NominalValue)
                .HasPrecision(11, 2)
                .IsRequired();
        }
    }
}
