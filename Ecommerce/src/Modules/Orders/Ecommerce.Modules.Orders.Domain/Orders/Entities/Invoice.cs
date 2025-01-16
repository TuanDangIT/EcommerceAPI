using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Invoice : AggregateRoot<int>, IAuditable
    {
        public string InvoiceNo { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Order Order { get; private set; } = new();
        public Guid OrderId { get; private set; }
        public Invoice(string invoiceNo, Order order)
        {
            InvoiceNo = invoiceNo;
            Order = order;
        }
        private Invoice()
        {

        }
    }
}
