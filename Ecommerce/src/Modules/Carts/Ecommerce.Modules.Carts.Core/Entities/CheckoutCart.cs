using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    internal class CheckoutCart
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Payment Payment { get; set; } = new();
        public Shipment Shipment { get; set; } = new();
        public List<CartProduct> _products = new();
        public IEnumerable<CartProduct> Products => _products;
        public CheckoutCart(Cart cart)
        {
            Id = cart.Id;
            CustomerId = cart.CustomerId;
            _products = cart.Products.ToList();
        }
        public void SetShipment(Shipment shipment)
            => Shipment = shipment;
        public void SetPayment(Payment payment)
            => Payment = payment;
    }
}
