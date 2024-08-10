using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Exceptions
{
    internal class ParameterNotAllDeletedException : EcommerceException
    {
        public ParameterNotAllDeletedException() : base("One or more parameters were not deleted.")
        {
        }
    }
}
