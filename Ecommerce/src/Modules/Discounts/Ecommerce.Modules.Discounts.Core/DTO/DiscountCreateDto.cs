using Ecommerce.Modules.Discounts.Core.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DTO
{
    public class DiscountCreateDto
    {
        [Required]
        [Range(2, 36)]
        public string Code { get; set; } = string.Empty;
        [FutureDateTime]
        public DateTime? EndingDate { get; set; }
    }
}
