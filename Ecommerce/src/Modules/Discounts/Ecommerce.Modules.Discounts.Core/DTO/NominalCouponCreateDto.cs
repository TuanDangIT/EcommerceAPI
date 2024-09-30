using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DTO
{
    public class NominalCouponCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(0.01, 999999)]
        public decimal NominalValue { get; set; }
    }
}
