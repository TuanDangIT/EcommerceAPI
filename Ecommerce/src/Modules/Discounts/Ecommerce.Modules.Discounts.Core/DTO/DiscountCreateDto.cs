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
        public string Code { get; set; } = string.Empty;
        public DateTime? EndingDate { get; set; }
    }
}
