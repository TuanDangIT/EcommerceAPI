using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public CustomerDto Customer { get; set; } = new();
        public decimal TotalSum { get; set; }
        public IEnumerable<ProductDto> Products { get; set; } = [];
        public string Payment { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? ClientAdditionalInformation { get; set; }
        public string? CompanyAdditionalInformation { get; set; }
        public DiscountDto? Discount { get; set; } = new();
        public IEnumerable<ShipmentDetailsDto> Shipment { get; set; } = [];
        public DateTime PlacedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
