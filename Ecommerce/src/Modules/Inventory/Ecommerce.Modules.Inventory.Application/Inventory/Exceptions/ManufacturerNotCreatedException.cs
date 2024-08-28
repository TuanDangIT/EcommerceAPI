using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Exceptions
{
    internal class ManufacturerNotCreatedException : EcommerceException
    {
        public ManufacturerNotCreatedException() : base("Manufacturer was not created.")
        {
        }
    }
}
