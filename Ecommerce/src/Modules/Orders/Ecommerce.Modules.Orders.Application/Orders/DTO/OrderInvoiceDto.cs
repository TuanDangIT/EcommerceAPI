using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class OrderInvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
    }
}
