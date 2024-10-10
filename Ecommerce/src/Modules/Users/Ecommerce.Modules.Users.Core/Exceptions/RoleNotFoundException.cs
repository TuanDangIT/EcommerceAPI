using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Exceptions
{
    internal class RoleNotFoundException : EcommerceException
    {
        public RoleNotFoundException(int roleId) : base($"Role: {roleId} was not found.")
        {
            
        }
    }
}
