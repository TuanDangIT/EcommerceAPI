using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Exceptions
{
    internal class CustomerNotFoundException(string email) : EcommerceException($"Customer with mail: {email} was not found.")
    {
    }
}
