using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Entities
{
    internal class Mail : BaseEntity<int>
    {
        public string From { get; private set; } = string.Empty;
        public string To { get; private set; } = string.Empty;
        public string Subject { get; private set; } = string.Empty;
        public string Body { get; private set; } = string.Empty;
        public Guid? OrderId { get; private set; }
        public IEnumerable<AttachmentFile>? AttachmentFiles { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Customer? Customer { get; private set; } 
        public Guid? CustomerId { get; private set; }
        public Mail(string from, string to, string subject, string body, Customer customer, Guid? orderId, IEnumerable<AttachmentFile>? attachmentFiles)
        {
            From = from;
            To = to; 
            Subject = subject; 
            Body = body;
            AttachmentFiles = attachmentFiles;
            Customer = customer;
            OrderId = orderId;
        }
        public Mail(string from, string to, string subject, string body, Guid? orderId, IEnumerable<AttachmentFile>? attachmentFiles)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            AttachmentFiles = attachmentFiles;
            OrderId = orderId;
        }
        private Mail()
        {
            
        }
    }
}
