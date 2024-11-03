using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.Exceptions
{
    internal class ComplaintCannotEditRefundAmountException : EcommerceException
    {
        public ComplaintCannotEditRefundAmountException() : base("Cannot edit refund amount on rejected complaint.")
        {
        }
    }
}
