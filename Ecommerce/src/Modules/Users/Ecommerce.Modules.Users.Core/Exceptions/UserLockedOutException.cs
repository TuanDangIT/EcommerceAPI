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
        public UserLockedOutException(TimeSpan timeSpan) : base($"Please try again in {timeSpan.Minutes + 1} minutes.")
        {
        }
    }
}
