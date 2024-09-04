using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ParameterInvalidNameLengthException : EcommerceException
    {
        public ParameterInvalidNameLengthException() : base("Parameter's name should be between 2 and 32 characters.")
        {
        }

    }
}
