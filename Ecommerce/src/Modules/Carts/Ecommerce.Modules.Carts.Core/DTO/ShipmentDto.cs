using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DTO
{
    public class ShipmentDto
    {
        [Required]
        [Length(2, 32)]
        public string City { get; set; } = string.Empty;
        [Required]
        [Length(2, 16)]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        [Length(2, 64)]
        public string StreetName { get; set; } = string.Empty;
        [Required]
        [Length(1, 8)]
        public string StreetNumber { get; set; } = string.Empty;
        [Required]
        [Length(1, 8)]
        public string AparmentNumber { get; set; } = string.Empty;
    }
}
