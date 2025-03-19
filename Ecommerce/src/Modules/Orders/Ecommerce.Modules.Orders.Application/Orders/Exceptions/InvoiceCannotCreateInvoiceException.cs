using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Exceptions
{
    internal class InvoiceCannotCreateInvoiceException(Guid orderId, string status) : EcommerceException($"Cannot create an invoice for order {orderId} because it is currently in the '{status}' status, which does not allow invoicing.")
    {
    }
}
