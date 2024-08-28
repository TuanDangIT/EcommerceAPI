using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Exceptions
{
    internal class CategoryNotAllDeletedException : EcommerceException
    {
        public CategoryNotAllDeletedException() : base("One or more categories were not deleted.")
        {
        }
    }
}
