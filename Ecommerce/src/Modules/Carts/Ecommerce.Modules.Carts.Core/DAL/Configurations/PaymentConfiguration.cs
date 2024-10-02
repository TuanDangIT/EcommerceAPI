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
            builder.HasData(
                new Payment(new Guid("db7346d0-b93e-402a-8025-75b393434c26"), PaymentMethod.card, true),
                new Payment(new Guid("4bb723c7-042a-4bed-9778-301a24f29f69"), PaymentMethod.paypal),
                new Payment(new Guid("157c6b6a-b4d6-423e-923b-e3ddf91f3e29"), PaymentMethod.blik),
                new Payment(new Guid("74d0d858-6854-4ea1-99c4-72296781b5d6"), PaymentMethod.google_pay),
                new Payment(new Guid("be53d850-12c8-4238-b5ee-51635f9944ff"), PaymentMethod.revolut_pay),
                new Payment(new Guid("30ad198c-160d-47ea-8c24-d3389392c3ac"), PaymentMethod.apple_pay),
                new Payment(new Guid("0bd5fec0-a5b1-4c2c-a3fa-2e5d5144daf1"), PaymentMethod.affirm),
                new Payment(new Guid("e78898ad-2f24-488b-acf8-d7a39d596f83"), PaymentMethod.cashapp)
                );
        }
    }
}
