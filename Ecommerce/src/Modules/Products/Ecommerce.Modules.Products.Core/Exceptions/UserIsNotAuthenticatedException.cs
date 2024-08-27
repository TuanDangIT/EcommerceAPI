using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Exceptions
{
    internal class UserIsNotAuthenticatedException : EcommerceException
    {
        public UserIsNotAuthenticatedException() : base("User is not authenticated.")
        {
        }
    }
}
