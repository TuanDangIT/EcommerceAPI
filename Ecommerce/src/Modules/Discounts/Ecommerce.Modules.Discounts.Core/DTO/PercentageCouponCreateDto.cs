using Ecommerce.Modules.Discounts.Core.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DTO
{
    public class PercentageCouponCreateDto
    {
        [Required]
        [Length(2, 36)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(0.01, 1)]
        public decimal Percent { get; set; }
    }
}
