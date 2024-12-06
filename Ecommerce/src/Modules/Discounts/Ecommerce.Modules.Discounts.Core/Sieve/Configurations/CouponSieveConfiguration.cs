using Ecommerce.Modules.Discounts.Core.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Sieve.Configurations
{
    internal class CouponSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Coupon>(c => c.Id)
                .CanFilter();
            mapper.Property<Coupon>(c => c.CreatedAt)
                .CanFilter()
                .CanSort();
            mapper.Property<Coupon>(c => c.Type)
                .CanFilter();
            mapper.Property<Coupon>(c => c.Name)
                .CanFilter()
                .CanSort();
            mapper.Property<Coupon>(c => c.Redemptions)
               .CanSort();
            mapper.Property<NominalCoupon>(nc => nc.NominalValue) 
                .CanFilter()
                .CanSort();
            mapper.Property<PercentageCoupon>(pc => pc.Percent)
                .CanFilter()
                .CanSort();
        }
    }
}
