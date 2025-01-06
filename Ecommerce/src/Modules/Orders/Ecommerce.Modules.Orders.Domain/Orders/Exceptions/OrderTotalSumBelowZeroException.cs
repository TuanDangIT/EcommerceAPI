using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal class OrderTotalSumBelowZeroException : EcommerceException
    {
        public OrderTotalSumBelowZeroException() : base("Order's total sum must be higher or equal 0.")
        {
        }
    }
}
