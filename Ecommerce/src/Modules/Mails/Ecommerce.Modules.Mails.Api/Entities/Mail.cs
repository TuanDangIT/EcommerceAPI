using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Entities
{
    internal class Mail
    {
        public int Id { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        //public string? PdfUrlPath { get; set; }
        public Guid? OrderId { get; set; }
        //public IEnumerable<string>? AttachmentFileNames { get; set; } 
        public DateTime CreatedAt { get; set; }
        public Customer? Customer { get; set; } 
        public Guid? CustomerId { get; set; }
        //public IEnumerable<Stream> Streams { get; set; } = [];
        public Mail(string from, string to, string subject, string body, Customer customer, Guid? orderId/*, IEnumerable<string>? attachmentFileNames*/)
        {
            From = from;
            To = to; 
            Subject = subject; 
            Body = body;
            Customer = customer;
            //AttachmentFileNames = attachmentFileNames;
            OrderId = orderId;
        }
        public Mail(string from, string to, string subject, string body, Guid? orderId/*, IEnumerable<string>? attachmentFileNames*/)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            //AttachmentFileNames = attachmentFileNames;
            OrderId = orderId;
        }
        public Mail()
        {
            
        }
    }
}
