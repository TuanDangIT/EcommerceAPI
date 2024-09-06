using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class CheckoutCart
    {
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Payment? Payment { get; set; }
        public Guid? PaymentId { get; set; }
        public Shipment? Shipment { get; set; }
        public string? StripeSessionId { get; set; }
        public bool IsPaid { get; set; } = false;
        private List<CartProduct> _products = [];
        public IEnumerable<CartProduct> Products => _products;
        public CheckoutCart(Cart cart)
        {
            Id = cart.Id;
            CustomerId = cart.CustomerId ?? Guid.Empty;
            _products = cart.Products.ToList();
        }
        public CheckoutCart()
        {
            
        }
        public void SetShipment(Shipment shipment)
            => Shipment = shipment;
        public void SetPayment(Payment payment)
            => Payment = payment;
        public void SetStripeSessionId(string sessionId)
            => StripeSessionId = sessionId;
        public void SetPaid()
            => IsPaid = true;
    }
}
