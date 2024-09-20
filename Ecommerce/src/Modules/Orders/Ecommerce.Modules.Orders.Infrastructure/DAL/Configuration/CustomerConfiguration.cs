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
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(48);
            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(48);
            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(64);
            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(16);
        }
    }
}
