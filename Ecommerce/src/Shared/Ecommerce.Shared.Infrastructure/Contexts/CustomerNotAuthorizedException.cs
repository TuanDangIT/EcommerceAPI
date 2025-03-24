using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Contexts
{
    public class CustomerNotAuthorizedException : EcommerceException
    {
        public CustomerNotAuthorizedException(Guid customerId) : base($"Customer with ID: {customerId} is not authorized for this resource.")
        {
        }
    }
}
