using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class AddressDto
    {
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
