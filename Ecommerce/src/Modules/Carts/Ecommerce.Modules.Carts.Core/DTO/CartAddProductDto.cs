using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DTO
{
    public class CartAddProductDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
