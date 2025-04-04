using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Entities
{
    internal class AttachmentFile
    {
        public Ulid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public int MailId { get; set; }
        public Mail Mail { get; set; } = default!;
        public AttachmentFile(Ulid id, string fileName)
        {
            Id = id;
            FileName = fileName;    
        }
        private AttachmentFile()
        {
            
        }
    }
}
