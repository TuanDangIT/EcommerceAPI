using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Configurations
{
    internal class ReturnConfiguration : IEntityTypeConfiguration<Return>
    {
        public void Configure(EntityTypeBuilder<Return> builder)
        {
            builder.OwnsMany(r => r.Products, p =>
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
                p.Property(p => p.UnitPrice)
                    .HasPrecision(11, 2)
                    .IsRequired();
                p.Property(p => p.Status)
                    .IsRequired()
                    .HasConversion<string>();
                p.Property(p => p.ImagePathUrl)
                    .IsRequired();
                p.ToTable("ReturnProducts", rp =>
                {
                    rp.HasCheckConstraint("CK_ReturnProduct_Price", "\"Price\" >= 0");
                    rp.HasCheckConstraint("CK_ReturnProduct_Quantity", "\"Quantity\" > 0");
                    rp.HasCheckConstraint("CK_ReturnProduct_UnitPrice", "\"UnitPrice\" >= 0");
                    rp.HasCheckConstraint("CK_ReturnProduct_PriceComparison", "\"UnitPrice\" <= \"Price\"");
                });
            });
            builder.Property(c => c.CreatedAt)
                .IsRequired();
            builder.Property(c => c.ReasonForReturn)
                .HasMaxLength(32)
                .IsRequired();
            builder.Property(c => c.Status)
                 .IsRequired()
                 .HasConversion<string>();
            builder.HasOne(r => r.Order)
                .WithOne()
                .HasForeignKey<Return>(r => r.OrderId);
            builder
                .HasIndex(r => new { r.Id, r.CreatedAt });
            builder.Property(o => o.Version)
                .IsConcurrencyToken();
        }
    }
}
