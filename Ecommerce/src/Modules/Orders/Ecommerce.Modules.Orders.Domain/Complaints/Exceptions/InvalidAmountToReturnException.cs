using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Exceptions
{
    internal class InvalidAmountToReturnException : EcommerceException
    {
        public InvalidAmountToReturnException() : base("Refund amount cannot be higher than sum price of products.")
        {
        }
    }
}
