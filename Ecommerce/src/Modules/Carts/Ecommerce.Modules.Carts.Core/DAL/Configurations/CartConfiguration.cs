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
    internal class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasMany(c => c.Products)
                .WithOne(cp => cp.Cart)
                .HasForeignKey(cp => cp.CartId);
            builder.HasOne(c => c.Discount)
                .WithMany(d => d.Carts)
                .HasForeignKey(c => c.DiscountId);
            builder.ToTable(c => c.HasCheckConstraint("CK_Cart_TotalSum", "\"TotalSum\" >= 0"));
        }
    }
}
