using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class DiscountDto
    {
        public string Type { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal? Value { get; set; }
        public string? SKU { get; set; }
    }
}
