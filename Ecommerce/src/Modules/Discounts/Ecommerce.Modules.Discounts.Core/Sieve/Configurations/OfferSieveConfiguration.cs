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
            mapper.Property<Offer>(o => o.Id)
                .CanFilter();
            mapper.Property<Offer>(o => o.CreatedAt)
                .CanFilter()
                .CanSort();
            //mapper.Property<Offer>(o => o.Status)
            //    .CanFilter();
            mapper.Property<Offer>(o => o.CustomerId)
                .CanFilter();
            mapper.Property<Offer>(o => o.OfferedPrice)
                .CanFilter();
            mapper.Property<Offer>(o => o.OldPrice)
                .CanFilter();
            mapper.Property<Offer>(o => o.Reason)
                .CanFilter();
            mapper.Property<Offer>(o => o.SKU)
                .CanFilter();
            mapper.Property<Offer>(o => o.ProductName)
                .CanFilter();
            mapper.Property<Offer>(o => o.Code)
                .CanFilter();
            mapper.Property<Offer>(o => o.CustomerId)
                .CanFilter();
        }
    }
}
