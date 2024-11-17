using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class ShipmentCursorDto : CursorDto<int>
    {
        //public int? CursorId { get; set; }
        public DateTime? CursorLabelCreatedAt { get; set; }
    }
}
