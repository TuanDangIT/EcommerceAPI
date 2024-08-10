using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Exceptions
{
    internal class InvalidCredentialsException : EcommerceException
    {
        public InvalidCredentialsException() : base("Email or password is invalid.")
        {
        }
    }
}
