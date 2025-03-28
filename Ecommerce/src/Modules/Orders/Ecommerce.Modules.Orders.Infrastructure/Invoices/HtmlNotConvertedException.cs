using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Invoices
{
    internal class HtmlNotConvertedException : EcommerceException
    {
        public HtmlNotConvertedException() : base("File was incorrectly converted to PDF from Html.")
        {
        }
    }
}
