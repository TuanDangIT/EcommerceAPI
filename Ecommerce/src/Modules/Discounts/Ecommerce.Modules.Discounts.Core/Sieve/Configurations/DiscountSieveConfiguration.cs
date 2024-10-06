using Ecommerce.Modules.Discounts.Core.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Sieve.Configurations
{
    internal class DiscountSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Discount>(d => d.Code)
                .CanSort()
                .CanFilter();
            mapper.Property<Discount>(d => d.ExpiresAt)
                .CanSort();
            mapper.Property<Discount>(d => d.CreatedAt)
                .CanSort();
            mapper.Property<Discount>(d => d.IsActive)
                .CanFilter();
            mapper.Property<Discount>(d => d.Redemptions)
                .CanSort();
        }
    }
}
