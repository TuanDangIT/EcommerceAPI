using Ecommerce.Modules.Orders.Domain.Complaints.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.DTO
{
    public class ComplaintBrowseDto
    {
        public Guid Id { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
        public string Title { get; set; } = string.Empty;
        public ComplainStatus Status { get; set; } = ComplainStatus.NotDecided;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
