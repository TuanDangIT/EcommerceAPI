using Ecommerce.Modules.Discounts.Core.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DTO
{
    public class NominalDiscountCreateDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        [Range(0, int.MaxValue)]
        public decimal NominalValue { get; set; }
        [FutureDateTime]
        public DateTime? EndingDate { get; set; }
    }
}
