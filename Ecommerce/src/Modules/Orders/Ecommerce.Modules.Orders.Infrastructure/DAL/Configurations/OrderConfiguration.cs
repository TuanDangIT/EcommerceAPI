using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Shipping.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsMany(o => o.Products, p =>
            {
                p.Property(p => p.SKU)
                    .HasMaxLength(16)
                    .IsRequired();
                p.Property(p => p.Name)
                    .HasMaxLength(24)
                    .IsRequired();
                p.Property(p => p.Price)
                    .HasPrecision(11, 2)
                    .IsRequired();
                p.Property(p => p.ImagePathUrl)
                    .IsRequired();
                p.ToTable("Products");
            });
            //builder.OwnsOne(o => o.ShipmentDetails, s =>
            //{
            //    s.Property(s => s.City).IsRequired().HasMaxLength(32);
            //    s.Property(s => s.PostalCode).IsRequired().HasMaxLength(16);
            //    s.Property(s => s.StreetName).IsRequired().HasMaxLength(64);
            //    s.Property(s => s.StreetNumber).IsRequired().HasMaxLength(8);
            //    s.Property(s => s.ApartmentNumber).HasMaxLength(8);
            //});
            builder.HasOne(o => o.Customer)
                .WithOne(c => c.Order)
                .HasForeignKey<Customer>(o => o.OrderId);
            builder.Property(o => o.Payment)
                .IsRequired()
                .HasConversion<string>();
            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<string>();
            builder.Property(cc => cc.ClientAdditionalInformation)
                .HasMaxLength(256);
            builder.Property(cc => cc.CompanyAdditionalInformation)
                .HasMaxLength(256);
            //builder.Property(o => o.IsCompleted)
            //    .IsRequired();
            builder.Property(o => o.CreatedAt)
                .IsRequired();
            builder.HasIndex(o => new { o.Id, o.CreatedAt });
            builder.HasOne(o => o.Shipment)
                .WithOne(s => s.Order)
                .HasForeignKey<Shipment>(s => s.OrderId);
            builder.Property(o => o.Version)
                .IsConcurrencyToken();
        }
    }
}
