using Ecommerce.Modules.Carts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DAL.Configuration
{
    internal class CheckoutCartConfiguration : IEntityTypeConfiguration<CheckoutCart>
    {
        public void Configure(EntityTypeBuilder<CheckoutCart> builder)
        {
            builder.OwnsOne(cc => cc.Shipment, s =>
            {
                s.Property(s => s.City).IsRequired().HasMaxLength(32);
                s.Property(s => s.PostalCode).IsRequired().HasMaxLength(16);
                s.Property(s => s.StreetName).IsRequired().HasMaxLength(64);
                s.Property(s => s.StreetNumber).IsRequired().HasMaxLength(8);
                s.Property(s => s.AparmentNumber).HasMaxLength(8);
                s.Property(s => s.ReceiverFullName).IsRequired().HasMaxLength(32);
            });
            builder.Property(cc => cc.IsPaid)
                .IsRequired();
            builder.HasOne(cc => cc.Payment)
                .WithMany(p => p.CheckoutCarts)
                .HasForeignKey(cc => cc.PaymentId);
            builder.HasMany(cc => cc.Products)
                .WithOne(cp => cp.CheckoutCart)
                .HasForeignKey(cp => cp.CheckoutCartId);
        }
    }
}
