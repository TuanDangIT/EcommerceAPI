using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.DTO
{
    public class OrderBrowseDto
    {
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal TotalSum { get; set; }
        public DateTime OrderPlacedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
