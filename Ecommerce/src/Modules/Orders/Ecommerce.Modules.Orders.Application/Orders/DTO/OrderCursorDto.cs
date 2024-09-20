using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class OrderCursorDto
    {
        public Guid? CursorId { get; set; }
        public DateTime? CursorOrderPlacedAt { get; set; }
    }
}
