using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DTO
{
    public class CustomerDto
    {
        [SwaggerIgnore]
        public Guid? CustomerId { get; set; }
        [Required]
        [Length(2, 48)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [Length(2, 48)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [Length(2, 64)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Length(2, 16)]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
