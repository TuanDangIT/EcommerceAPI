using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DTO
{
    internal class AttachmentFileDto
    {
        public Ulid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
    }
}
