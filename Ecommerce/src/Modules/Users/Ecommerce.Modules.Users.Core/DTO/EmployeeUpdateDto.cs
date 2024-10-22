using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DTO
{
    public class EmployeeUpdateDto
    {
        [Required]
        public Guid EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string JobPosition { get; set; } = string.Empty;
        [EmailAddress]
        [Length(2, 64)]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Length(2, 16)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [AllowedValues("Manager", "Employee",
            ErrorMessage = "You only have 2 options: Manager or Employee.")]
        public string Role { get; set; } = string.Empty;
    }
}
