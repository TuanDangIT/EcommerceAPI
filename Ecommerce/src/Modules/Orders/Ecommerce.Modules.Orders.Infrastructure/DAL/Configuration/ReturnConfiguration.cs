using Ecommerce.Modules.Orders.Domain.Returns.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Configuration
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
                p.Property(p => p.ImagePathUrl)
                    .IsRequired();
                p.ToTable("ReturnProducts");
            });
            builder.HasOne(o => o.Customer)
                .WithOne(c => c.Return)
                .HasForeignKey(nameof(Return));
            builder.Property(c => c.CreatedAt)
                .IsRequired();
            builder.Property(c => c.ReasonForReturn)
                .HasMaxLength(32)
                .IsRequired();
            builder.Property(c => c.Status)
                 .IsRequired()
                 .HasConversion<string>();
        }
    }
}
