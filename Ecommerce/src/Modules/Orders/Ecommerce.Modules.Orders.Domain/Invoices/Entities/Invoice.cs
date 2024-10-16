using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Invoices.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public string InvoiceUrlPath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public Invoice(string invoiceNo, string invoiceUrlPath, Order order, DateTime createdAt)
        {
            InvoiceNo = invoiceNo;
            InvoiceUrlPath = invoiceUrlPath;
            Order = order;
            CreatedAt = createdAt;

        }
        public Invoice()
        {
            
        }
    }
}
