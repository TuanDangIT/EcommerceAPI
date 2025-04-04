using Ecommerce.Modules.Mails.Api.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Sieve.Configurations
{
    internal class MailSieveConfiguration : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Mail>(m => m.OrderId)
                .CanFilter();
            mapper.Property<Mail>(m => m.Customer)
                .CanFilter();
            mapper.Property<Mail>(m => m.CreatedAt)
                .CanFilter()
                .CanSort();
            mapper.Property<Mail>(m => m.Subject)
                .CanFilter();
            mapper.Property<Mail>(m => m.To)
                .CanFilter();
            mapper.Property<Mail>(m => m.Body)
                .CanFilter();
        }
    }
}
