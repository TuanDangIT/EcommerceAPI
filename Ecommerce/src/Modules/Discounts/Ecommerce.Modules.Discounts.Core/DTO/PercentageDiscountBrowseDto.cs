using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DTO
{
    public class PercentageDiscountBrowseDto
    {
        public string Code { get; set; } = string.Empty;
        //public string Type { get; set; } = string.Empty;
        public decimal Percent { get; set; }
        public DateTime? EndingDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
