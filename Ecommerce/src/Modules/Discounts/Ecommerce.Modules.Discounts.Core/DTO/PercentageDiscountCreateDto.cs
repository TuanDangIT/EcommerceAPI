using Ecommerce.Modules.Discounts.Core.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DTO
{
    public class PercentageDiscountCreateDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        [Range(0, 1)]
        public decimal Percent { get; set; }
        [FutureDateTime]
        public DateTime? EndingDate { get; set; }
    }
}
