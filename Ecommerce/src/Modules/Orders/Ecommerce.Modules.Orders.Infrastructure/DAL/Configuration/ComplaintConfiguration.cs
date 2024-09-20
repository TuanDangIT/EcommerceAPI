using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Configuration
{
    internal class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
    {
        public void Configure(EntityTypeBuilder<Complaint> builder)
        {
            builder.OwnsOne(c => c.Decision, d => 
            {
                d.Property(d => d.DecisionText)
                    .IsRequired();
            });
            builder.HasOne(o => o.Customer)
                .WithOne(c => c.Complaint)
                .HasForeignKey(nameof(Complaint));
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
        }
    }
}
