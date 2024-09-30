using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DTO
{
    public class NominalCouponBrowseDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal NominalValue { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
