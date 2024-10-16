using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Invoices.Exceptions
{
    internal class InvoiceNotFoundException : EcommerceException
    {
        public InvoiceNotFoundException(int invoiceId) : base($"Invoice: {invoiceId} was not found.")
        {
        }
    }
}
