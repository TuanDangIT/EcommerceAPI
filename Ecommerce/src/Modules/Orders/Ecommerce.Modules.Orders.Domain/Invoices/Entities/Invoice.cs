using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Invoices.Entities
{
    public class Invoice : AggregateRoot<int>, IAuditable
    {
        public string InvoiceNo { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public InvoiceCustomer Customer { get; private set; } = new();
        public Order Order { get; private set; } = new();
        public Guid OrderId { get; private set; }
        public Invoice(string invoiceNo, Order order, InvoiceCustomer customer)
        {
            InvoiceNo = invoiceNo;
            Order = order;
            Customer = customer;
        }
        public Invoice()
        {
            
        }
        public void EditCustomer(InvoiceCustomer customer)
        {
            Customer = customer;
            IncrementVersion();
        }
    }
}
