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
        public Guid? CustomerId { get; set; }
        public Payment? Payment { get; set; } = new();
        public Shipment? Shipment { get; set; } = new();
        public bool IsPaid { get; set; } = false;
        public IEnumerable<CartProductDto> Products { get; set; } = [];
    }
}
