using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Mails
{
    public class MailOptions
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; } 
    }
}
