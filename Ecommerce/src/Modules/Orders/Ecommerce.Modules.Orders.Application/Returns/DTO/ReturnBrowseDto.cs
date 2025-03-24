using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.DTO
{
    public class ReturnBrowseDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string CustomerFullName { get; set; } = string.Empty;
        public string ReasonForReturn { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
