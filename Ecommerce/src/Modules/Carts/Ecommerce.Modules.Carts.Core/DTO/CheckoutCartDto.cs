using Ecommerce.Modules.Carts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DTO
{
    public class CheckoutCartDto
    {
        public Guid Id { get; set; }
        public CustomerDto Customer { get; set; } = new();
        public PaymentDto? Payment { get; set; } = new();
        public ShipmentDto? Shipment { get; set; } = new();
        public string? AdditionalInformation { get; set; } = string.Empty;
        public bool IsPaid { get; set; } = false;
        public decimal TotalSum { get; set; }
        public IEnumerable<CartProductDto> Products { get; set; } = [];
        public DiscountDto? Discount { get; set; } = new();
    }
}
