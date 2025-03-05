using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Exceptions
{
    internal class CannotActivateExpiredDiscountException : EcommerceException
    {
        public CannotActivateExpiredDiscountException(string code) : base("Cannot activate expired discount: {code}.")
        {
        }
    }
}
