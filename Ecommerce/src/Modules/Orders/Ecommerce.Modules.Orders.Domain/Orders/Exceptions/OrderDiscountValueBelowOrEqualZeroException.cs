using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal class OrderDiscountValueBelowOrEqualZeroException : EcommerceException
    {
        public OrderDiscountValueBelowOrEqualZeroException() : base("Order's discount value must be higher than 0.")
        {
        }
    }
}
