﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DTO
{
    public class CustomerUpdateDto
    {
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        [Length(2, 64)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [Length(2, 64)]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        [Length(2, 64)]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Length(2, 16)]
        public string Username { get; set; } = string.Empty;
    }
}
