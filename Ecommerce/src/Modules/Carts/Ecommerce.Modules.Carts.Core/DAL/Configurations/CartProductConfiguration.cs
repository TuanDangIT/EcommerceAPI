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
    internal class CartProductConfiguration : IEntityTypeConfiguration<CartProduct>
    {
        public void Configure(EntityTypeBuilder<CartProduct> builder)
        {
            //builder.Property(cp => cp.Quantity)
            //    .IsRequired();
            builder.Property(cp => cp.ProductId)
                .IsRequired();
            builder.Property(cp => cp.CartId)
                .IsRequired();
        }
    }
}
