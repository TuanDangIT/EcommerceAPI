using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Auctions.Sieve.Configurations
{
    internal class SieveConfigurationForReview : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Review>(r => r.Grade)
                .CanSort();
        }
    }
}
