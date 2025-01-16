using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Exception
{
    internal class ReturnNotFoundException : EcommerceException
    {
        public ReturnNotFoundException(Guid orderId) : base($"Return: {orderId} was not found.")
        {
        }
    }
}
