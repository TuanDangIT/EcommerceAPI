using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.DTO
{
    public class ReturnDetailsDto
    {
        public Guid Id { get; set; }
        public OrderShortenedDetailsDto Order { get; set; } = new();
        public IEnumerable<ReturnProductDto> Products { get; set; } = [];
        public decimal TotalSum { get; set; }
        public decimal TotalSumLeftToReturn { get; set; }
        public string ReasonForReturn { get; set; } = string.Empty;
        public string? AdditionalNote { get; set; }
        public string? RejectReason { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
