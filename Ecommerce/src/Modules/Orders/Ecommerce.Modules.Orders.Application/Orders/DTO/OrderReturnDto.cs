using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class OrderReturnDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public int NumberOfProductsReturned { get; set; }
        public decimal TotalSum { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
