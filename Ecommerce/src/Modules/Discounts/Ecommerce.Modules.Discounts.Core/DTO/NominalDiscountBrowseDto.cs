using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DTO
{
    public class NominalDiscountBrowseDto
    {
        public string Code { get; set; } = string.Empty;
        //public string Type { get; set; } = string.Empty;
        public decimal NominalValue { get; set; }
        public DateTime? EndingDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
