using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class ShipmentBrowseDto
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public string LabelId { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public string ShippingService { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
