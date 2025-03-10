using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerSchema("Nominal coupon's name")]
        [Required]
        [Length(2, 36)]
        public string Name { get; set; } = string.Empty;
        [SwaggerSchema("Nominal coupon's value")]
        [Required]
        [Range(0.01, 999999)]
        public decimal NominalValue { get; set; }
    }
}
