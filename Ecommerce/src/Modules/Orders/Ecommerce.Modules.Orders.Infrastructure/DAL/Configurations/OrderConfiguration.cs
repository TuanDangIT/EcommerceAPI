using Ecommerce.Modules.Orders.Domain.Orders.Entities;
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
                    .HasMaxLength(64)
                    .IsRequired();
                p.Property(p => p.Price)
                    .HasPrecision(11, 2)
                    .IsRequired();
                p.ToTable("Products", p =>
                {
                    p.HasCheckConstraint("CK_Product_UnitPrice", "\"UnitPrice\" >= 0");
                    p.HasCheckConstraint("CK_Product_Price", "\"Price\" >= 0");
                    p.HasCheckConstraint("CK_Product_Quantity", "\"Quantity\" >= 0");
                });
            });
            builder.OwnsOne(o => o.Discount, d =>
            {
                d.Property(d => d.Type).IsRequired().HasConversion<string>();
                d.Property(d => d.Code).IsRequired().HasMaxLength(48);
                d.Property(d => d.SKU).HasMaxLength(16);
            });
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
            builder.Property(o => o.CreatedAt)
                .IsRequired();
            builder.HasIndex(o => new { o.Id, o.CreatedAt });
            builder.HasMany(o => o.Shipments)
                .WithOne(s => s.Order)
                .HasForeignKey(s => s.OrderId);
            builder.Property(o => o.Version)
                .IsConcurrencyToken();
            builder.ToTable(o =>
            {
                o.HasCheckConstraint("CK_Order_TotalSum", "\"TotalSum\" >= 0");
                o.HasCheckConstraint("CK_Order_ShippingPrice", "\"ShippingPrice\" >= 0");
                o.HasCheckConstraint("CK_Order_DiscountValue", "\"Discount_Value\" > 0");
            });
        }
    }
}
