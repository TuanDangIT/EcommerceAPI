using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class AddressDto
    {
        public string Street { get; private set; } = string.Empty;
        public string BuildingNumber { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string PostCode { get; private set; } = string.Empty;
    }
}
