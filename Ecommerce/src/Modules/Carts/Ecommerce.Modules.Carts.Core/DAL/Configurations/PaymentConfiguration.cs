using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DAL.Configurations
{
    internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasConversion<string>();
            builder.HasData(new Payment(new Guid("db7346d0-b93e-402a-8025-75b393434c26"), PaymentMethod.card));
        }
    }
}
