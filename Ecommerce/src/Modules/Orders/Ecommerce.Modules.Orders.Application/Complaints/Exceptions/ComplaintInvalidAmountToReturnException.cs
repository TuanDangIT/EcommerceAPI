using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Exceptions
{
    internal class ComplaintInvalidAmountToReturnException : EcommerceException
    {
        public ComplaintInvalidAmountToReturnException() : base("Amount to return from complaint should be higher than 0 and less than price of an order.")
        {
        }
    }
}
