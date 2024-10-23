using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DTO
{
    public class MailSendDefaultBodyDto
    {
        public string To { get; set; } = string.Empty;
        public string? FirstName { get; set; } 
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public IEnumerable<IFormFile>? Files { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
