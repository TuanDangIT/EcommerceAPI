using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DTO
{
    public class PercentageCouponBrowseDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Percent { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
