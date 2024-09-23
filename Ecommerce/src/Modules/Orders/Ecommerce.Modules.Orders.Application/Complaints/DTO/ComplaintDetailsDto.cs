using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Domain.Complaints.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.DTO
{
    public class ComplaintDetailsDto
    {
        public Guid Id { get; set; }
        public CustomerDto Customer { get; set; } = new();
        public OrderShortenedDetailsDto Order { get; set; } = new();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? AdditionalNote { get; set; }
        public DecisionDto? Decision { get; set; } = new();
        public ComplainStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
