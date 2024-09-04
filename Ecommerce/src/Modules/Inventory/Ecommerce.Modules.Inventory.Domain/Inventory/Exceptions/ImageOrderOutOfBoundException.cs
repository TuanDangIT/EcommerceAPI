using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ImageOrderOutOfBoundException : EcommerceException
    {
        public ImageOrderOutOfBoundException() : base("Image's order is out of bound. Max is 8.")
        {
        }
    }
}
