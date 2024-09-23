using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Complaints.DTO
{
    public class DecisionDto
    {
        public string DecisionText { get; set; } = string.Empty;
        public string? AdditionalInformation { get; set; } = string.Empty;
    }
}
