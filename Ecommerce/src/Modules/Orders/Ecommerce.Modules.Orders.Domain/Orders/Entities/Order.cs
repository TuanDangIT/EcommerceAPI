using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; } = new();
        private readonly List<Product> _products = [];
        public IEnumerable<Product> Products => _products;
        public Shipment Shipment { get; set; } = new();
        public PaymentMethod Payment { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Placed;
        public bool IsCompleted => Status is OrderStatus.Cancelled || Status is OrderStatus.Completed;
        public string? AdditionalInformation { get; set; }
        public DateTime OrderPlacedAt { get; set; }
        public Order(Guid id, Customer customer, IEnumerable<Product> products, Shipment shipment,
            PaymentMethod paymentMethod, DateTime orderPlacedAt, string? additionalInformation = null)
        {
            Id = id;
            Customer = customer;
            _products = products.ToList();
            Shipment = shipment;
            Payment = paymentMethod;
            OrderPlacedAt = orderPlacedAt;
            AdditionalInformation = additionalInformation;
        }
        public Order()
        {
            
        }
    }
}
