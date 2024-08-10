using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Exceptions
{
    internal class InvalidRefreshToken : EcommerceException
    {
        public InvalidRefreshToken() : base("Refresh token is invalid.")
        {
        }
    }
}
