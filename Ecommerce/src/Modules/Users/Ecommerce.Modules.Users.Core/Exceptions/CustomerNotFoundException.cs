using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Exceptions
{
    internal class CustomerNotFoundException(Guid customerId) : EcommerceException($"Customer: {customerId} was not found.")
    {
    }
}
