using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Exceptions
{
    internal class UserLockedOutException : EcommerceException
    {
        public UserLockedOutException(TimeSpan minutesLeft) : base($"Please try again in {minutesLeft.Minutes} minutes.")
        {
        }
    }
}
