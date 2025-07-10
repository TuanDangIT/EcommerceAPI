using Ecommerce.Modules.Carts.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DAL.Configurations
{
    internal class DeliveryServiceConfiguration : IEntityTypeConfiguration<DeliveryService>
    {
        public void Configure(EntityTypeBuilder<DeliveryService> builder)
        {
            builder.HasData(
                new DeliveryService(1, Entities.Enums.Courier.InPost, "Kurier InPost", 3),
                new DeliveryService(2, Entities.Enums.Courier.DPD, "Kurier DPD", 3.5m)
            );

            builder.ToTable(ds =>
            {
                ds.HasCheckConstraint("CK_DeliveryService_Price", "\"Price\" >= 0");
            });
        }
    }
}
