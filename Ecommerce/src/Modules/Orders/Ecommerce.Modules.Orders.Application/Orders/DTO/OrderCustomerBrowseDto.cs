using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class OrderCustomerBrowseDto
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; } 
        public string Status { get; set; } = string.Empty;
        public decimal TotalSum { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
