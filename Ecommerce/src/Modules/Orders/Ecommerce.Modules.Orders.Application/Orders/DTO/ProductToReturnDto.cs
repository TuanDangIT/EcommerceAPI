using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class ProductToReturnDto
    {
        public string SKU { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
