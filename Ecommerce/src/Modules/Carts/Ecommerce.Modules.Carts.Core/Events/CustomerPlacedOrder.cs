using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Shared.Abstractions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Events
{
    public sealed record class CustomerPlacedOrder : IEvent
    {
        public Guid? CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public IEnumerable<object> Products { get; set; } = Enumerable.Empty<object>();
        public decimal TotalSum { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string StreetName { get; set; } = string.Empty;
        public string StreetNumber { get; set; } = string.Empty;
        public string? ApartmentNumber { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? AdditionalInformation {  get; set; } 
        public string? DiscountCode { get; set; } 
        public string StripePaymentIntentId { get; set; } = string.Empty;
    };
}
