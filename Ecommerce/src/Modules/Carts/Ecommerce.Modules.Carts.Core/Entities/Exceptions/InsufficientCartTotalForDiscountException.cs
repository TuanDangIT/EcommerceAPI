using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class InsufficientCartTotalForDiscountException : EcommerceException
    {
        public InsufficientCartTotalForDiscountException(decimal requiredCartTotalValue, decimal totalSum) 
            : base($"The cart total of {totalSum} is insufficient. The minimum required total value for discount eligibility is {requiredCartTotalValue}.")
        {
            
        }
    }
}
