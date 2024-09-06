using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Events.External
{
    public sealed record class CustomerPlacedOrder : IEvent
    {
        public Guid? CustomerId { get; set; }
        public IEnumerable<object> Products { get; set; } = [];
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string StreetName { get; set; } = string.Empty;
        public string StreetNumber { get; set; } = string.Empty;
        public string AparmentNumber { get; set; } = string.Empty;
        public string ReceiverFullName { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
