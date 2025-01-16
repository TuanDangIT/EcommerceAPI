using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Exceptions
{
    internal class ReturnCannotHandleException : EcommerceException
    {
        public ReturnCannotHandleException() : base("Return cannot be handled, because not all products in it are accepted.")
        {
        }
    }
}
