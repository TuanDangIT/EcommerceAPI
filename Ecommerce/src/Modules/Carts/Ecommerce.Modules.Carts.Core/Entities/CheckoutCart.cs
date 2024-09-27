using Ecommerce.Modules.Carts.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class CheckoutCart
    {
        public Guid Id { get; private set; }
        public Customer Customer { get; private set; } = new();
        public Payment? Payment { get; private set; }
        public Guid? PaymentId { get; private set; }
        public Shipment? Shipment { get; private set; }
        public string? AdditionalInformation { get; private set; }
        public string? StripePaymentIntentId { get; private set; }
        public string? StripeSessionId { get; private set; }
        public bool IsPaid { get; private set; } = false;
        private readonly List<CartProduct> _products = [];
        public IEnumerable<CartProduct> Products => _products;
        public Discount? Discount { get; private set; }
        public int? DiscountId { get; private set; }
        public CheckoutCart(Cart cart)
        {
            Id = cart.Id;
            Customer.SetCustomerId(cart.CustomerId ?? Guid.Empty);
            _products = cart.Products.ToList();
        }
        public CheckoutCart()
        {
            
        }
        public decimal TotalSum()
        {
            var productsTotal = _products.Sum(cp => cp.Product.Price * cp.Quantity);
            if (Discount is null)
            {
                return productsTotal;
            }
            if(Discount.Type is DiscountType.NominalDiscount)
            {
                return productsTotal - Discount.Value;
            }
            return productsTotal * Discount.Value;
        }
        public void SetCustomer(Customer customer)
        {
            Customer.SetCustomerDetails(customer.FirstName, customer.LastName, customer.Email, customer.PhoneNumber);
        }
        public void SetShipment(Shipment shipment)
            => Shipment = shipment;
        public void SetPayment(Payment payment)
            => Payment = payment;
        public void SetAdditionalInformation(string additionalInformation)
            => AdditionalInformation = additionalInformation;
        public void SetStripeSessionId(string sessionId)
            => StripeSessionId = sessionId;
        public void SetStripePaymentIntentId(string stripePaymentIntentId)
            => StripePaymentIntentId = stripePaymentIntentId;
        public void SetPaid()
            => IsPaid = true;
        public void AddDiscount(Discount discount)
            => Discount = discount;
    }
}
