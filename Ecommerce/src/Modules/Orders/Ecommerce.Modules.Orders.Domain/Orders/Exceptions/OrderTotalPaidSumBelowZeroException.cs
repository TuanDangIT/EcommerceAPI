using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Exceptions
{
    internal class OrderTotalPaidSumBelowZeroException : EcommerceException
    {
        public OrderTotalPaidSumBelowZeroException() : base("Order total paid sum cannot be below zero.")
        {
        }
    }
}
