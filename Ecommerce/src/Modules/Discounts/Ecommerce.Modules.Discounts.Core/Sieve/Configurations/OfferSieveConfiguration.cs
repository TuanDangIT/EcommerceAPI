using Ecommerce.Modules.Discounts.Core.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Sieve.Configurations
{
    internal class OfferSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Offer>(o => o.CreatedAt)
                .CanFilter()
                .CanSort();
            mapper.Property<Offer>(o => o.Status)
                .CanFilter();
            mapper.Property<Offer>(o => o.CustomerId)
                .CanFilter();
        }
    }
}
