using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Exceptions
{
    internal class UsernameInUseException : EcommerceException
    {
        public UsernameInUseException(string message) : base(message)
        {
        }
        public UsernameInUseException() : base("Username is already in use.")
        {
        }
    }
}
