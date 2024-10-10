using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DTO
{
    public class EmployeeCreateDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [EmailAddress]
        [Length(2, 64)]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Length(8, 64)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
            ErrorMessage = "Password must contain 8 characters, at least one uppercase letter, one lowercase letter, one digit and one special character (#?!@$%^&*-).")]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Length(8, 64)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        [Length(2, 16)]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string JobPosition { get; set; } = string.Empty;
        [Required]
        [AllowedValues("Manager", "Employee",
            ErrorMessage = "You only have 2 options: Manager or Employee.")]
        public string Role { get; set; } = string.Empty;
    }
}
