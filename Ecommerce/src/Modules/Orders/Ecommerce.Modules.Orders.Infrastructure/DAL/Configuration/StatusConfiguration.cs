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
    internal class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.Property(s => s.OrderStatus)
                .IsRequired()
                .HasConversion<string>();
            builder.HasData(
                new Status(1, Domain.Orders.Entities.Enums.OrderStatus.Placed),
                new Status(2, Domain.Orders.Entities.Enums.OrderStatus.ParcelPacked),
                new Status(3, Domain.Orders.Entities.Enums.OrderStatus.Shipped),
                new Status(4, Domain.Orders.Entities.Enums.OrderStatus.Completed),
                new Status(5, Domain.Orders.Entities.Enums.OrderStatus.Cancelled),
                new Status(6, Domain.Orders.Entities.Enums.OrderStatus.Returned)
                );
        }
    }
}
