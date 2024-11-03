using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Configurations
{
    internal class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
    {
        public void Configure(EntityTypeBuilder<Complaint> builder)
        {
            builder.OwnsOne(c => c.Decision, d => 
            {
                d.Property(d => d.DecisionText)
                    .IsRequired();
                d.Property(d => d.RefundAmount)
                    .HasPrecision(11, 2);
            });
            builder.Property(c => c.CreatedAt)
                .IsRequired();
            builder.Property(c => c.Title)
                .HasMaxLength(32)
                .IsRequired();
            builder.Property(c => c.Description)
                .IsRequired();
            builder.Property(c => c.Status)
                 .IsRequired()
                 .HasConversion<string>();
            builder.HasOne(c => c.Order)
                .WithMany()
                .HasForeignKey(c => c.OrderId);
            builder
                .HasIndex(c => new { c.Id, c.CreatedAt });
        }
    }
}
