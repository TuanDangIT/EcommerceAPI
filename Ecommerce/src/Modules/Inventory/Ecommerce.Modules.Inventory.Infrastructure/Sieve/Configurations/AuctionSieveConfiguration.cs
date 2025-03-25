using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Infrastructure.Sieve.Configurations
{
    internal class AuctionSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Auction>(p => p.Id)
                .CanFilter();
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
            mapper.Property<Auction>(p => p.CreatedAt)
                .CanFilter()
                .CanSort();
        }
    }
}
