using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Exceptions
{
    internal class ComplaintCannotEditException : EcommerceException
    {
        public ComplaintCannotEditException(string message) : base(message)
        {
            
        }
    }
}
