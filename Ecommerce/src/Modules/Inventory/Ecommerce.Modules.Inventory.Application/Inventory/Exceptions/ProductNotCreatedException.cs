using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Exceptions
{
    internal class ProductNotCreatedException : EcommerceException
    {
        public ProductNotCreatedException() : base("Product was not created.")
        {
        }
    }
}
