using Ecommerce.Modules.Auctions.Core.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.Sieve.SieveConfiguration
{
    internal class SieveConfigurationForAuction : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Auction>(p => p.SKU)
                .CanFilter();
            mapper.Property<Auction>(p => p.Name)
                .CanFilter();
            mapper.Property<Auction>(p => p.Price)
                .CanFilter()
                .CanSort();
            mapper.Property<Auction>(p => p.Quantity)
                .CanFilter()
                .CanSort();
            mapper.Property<Auction>(p => p.Description)
                .CanFilter();
            mapper.Property<Auction>(p => p.AdditionalDescription)
                .CanFilter();
            mapper.Property<Auction>(p => p.Manufacturer)
                .CanFilter();
            mapper.Property<Auction>(p => p.Category)
                .CanFilter();
        }
    }
}
