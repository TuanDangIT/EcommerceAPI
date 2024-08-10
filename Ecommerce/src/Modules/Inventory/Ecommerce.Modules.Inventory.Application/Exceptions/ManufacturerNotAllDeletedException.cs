using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Exceptions
{
    internal class ManufacturerNotAllDeletedException : EcommerceException
    {
        public ManufacturerNotAllDeletedException() : base("One or more manufacturers were not deleted.")
        {
        }
    }
}
