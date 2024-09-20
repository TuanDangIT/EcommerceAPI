using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Exceptions
{
    internal class ReturnNotFoundException(Guid returnId) : EcommerceException($"Return: {returnId} was not found.")
    {
    }
}
