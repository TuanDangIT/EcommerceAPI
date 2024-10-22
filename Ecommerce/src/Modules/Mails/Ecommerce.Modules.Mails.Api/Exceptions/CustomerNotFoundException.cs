using Ecommerce.Modules.Mails.Api.Entities;
using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Exceptions
{
    internal class CustomerNotFoundException : EcommerceException
    {
        public CustomerNotFoundException(string email) : base($"Customer with mail: {email} was not found.")
        {
        }
        public CustomerNotFoundException(Guid customerId) : base($"Customer: {customerId} was not found.")
        {

        }
    }
}
