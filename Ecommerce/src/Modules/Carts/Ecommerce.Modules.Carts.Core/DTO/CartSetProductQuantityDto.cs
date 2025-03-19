using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DTO
{
    public class CartSetProductQuantityDto
    {
        [SwaggerIgnore]
        public Guid ProductId { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
