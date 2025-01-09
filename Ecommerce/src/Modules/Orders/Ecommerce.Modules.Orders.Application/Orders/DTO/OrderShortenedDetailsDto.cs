using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class OrderShortenedDetailsDto
    {
        public Guid Id { get; set; }
        public CustomerDto Customer { get; set; } = new();
        public decimal TotalSum { get; set; }
        public IEnumerable<ProductDto> Products { get; set; } = [];
        public DateTime PlacedAt { get; set; }
    }
}
