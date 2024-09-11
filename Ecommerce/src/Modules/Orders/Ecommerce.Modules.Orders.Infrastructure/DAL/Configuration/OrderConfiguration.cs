using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.Shipment, s =>
            {
                s.Property(s => s.City).IsRequired().HasMaxLength(32);
                s.Property(s => s.PostalCode).IsRequired().HasMaxLength(16);
                s.Property(s => s.StreetName).IsRequired().HasMaxLength(64);
                s.Property(s => s.StreetNumber).IsRequired().HasMaxLength(8);
                s.Property(s => s.AparmentNumber).HasMaxLength(8);
            });
            builder.OwnsOne(cc => cc.Customer, c =>
            {
                c.Property(c => c.FirstName).IsRequired().HasMaxLength(48);
                c.Property(c => c.LastName).IsRequired().HasMaxLength(48);
                c.Property(c => c.Email).IsRequired().HasMaxLength(64);
                c.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(16);
            });
            builder.Property(o => o.Payment)
                .IsRequired()
                .HasConversion<string>();
            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<string>();
            builder.Property(cc => cc.AdditionalInformation)
                .HasMaxLength(256);
            //builder.Property(o => o.IsCompleted)
            //    .IsRequired();
            builder.Property(o => o.OrderPlacedAt)
                .IsRequired();
            builder.HasMany(o => o.Products)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId);
        }
    }
}
