using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DTO
{
    public class CheckoutCartSetDetailsDto
    {
        public CustomerDto CustomerDto { get; set; } = new();
        public Guid PaymentId { get; set; } 
        public ShipmentDto ShipmentDto { get; set; } = new();
        public string? AdditionalInformation { get; set; } 
    }
}
