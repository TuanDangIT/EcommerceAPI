using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [MaxLength(256)]
        public string? AdditionalInformation { get; set; } 
    }
}
