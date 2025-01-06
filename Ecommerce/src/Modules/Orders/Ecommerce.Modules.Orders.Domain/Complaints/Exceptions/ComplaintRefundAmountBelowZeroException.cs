using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Exceptions
{
    internal class ComplaintRefundAmountBelowZeroException : EcommerceException
    {
        public ComplaintRefundAmountBelowZeroException() : base("Refund amount must be higher or equal 0.")
        {
        }
    }
}
