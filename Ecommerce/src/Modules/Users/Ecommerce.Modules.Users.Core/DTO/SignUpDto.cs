﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DTO
{
    public class SignUpDto
    {
        public Guid Id { get; set; }

        [EmailAddress]
        [Length(2, 64)]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Length(8, 64)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}$",
            ErrorMessage = "Password must contain 8 characters, at least one uppercase letter, one lowercase letter and one number.")]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Length(8, 64)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        [Length(2, 16)]
        public string Username { get; set; } = string.Empty;
        public string? Role { get; set; }
    }
}
