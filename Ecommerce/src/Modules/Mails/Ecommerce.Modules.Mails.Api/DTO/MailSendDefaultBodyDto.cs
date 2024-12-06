using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DTO
{
    public class MailSendDefaultBodyDto
    {
        [Required]
        [EmailAddress]
        public string To { get; set; } = string.Empty;
        [MinLength(1)]
        public string? FirstName { get; set; }
        [Required]
        [Length(1, 64)]
        public string Subject { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        public string Message { get; set; } = string.Empty;
        public IEnumerable<IFormFile>? Files { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
