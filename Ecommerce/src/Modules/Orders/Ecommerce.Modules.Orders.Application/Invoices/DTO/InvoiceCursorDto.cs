using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Invoices.DTO
{
    public class InvoiceCursorDto
    {
        public int? CursorId { get; set; }
        public DateTime? CursorCreatedAt { get; set; }
    }
}
