using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Exceptions
{
    internal class ParameterNotCreatedException : EcommerceException
    {
        public ParameterNotCreatedException() : base("Parameter was not created.")
        {
        }
    }
}
