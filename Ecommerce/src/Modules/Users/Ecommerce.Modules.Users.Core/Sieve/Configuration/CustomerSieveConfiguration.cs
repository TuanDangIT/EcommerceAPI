using Ecommerce.Modules.Users.Core.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Sieve.Configuration
{
    internal class CustomerSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Customer>(c => c.Id)
                .CanFilter();
            mapper.Property<Customer>(c => c.FirstName)
                .CanFilter();
            mapper.Property<Customer>(c => c.LastName)
                .CanFilter();
            mapper.Property<Customer>(c => c.Email)
                .CanFilter();
            mapper.Property<Customer>(c => c.Username)
                .CanFilter();
            mapper.Property<Customer>(c => c.CreatedAt)
                .CanFilter()
                .CanSort();
        }
    }
}
