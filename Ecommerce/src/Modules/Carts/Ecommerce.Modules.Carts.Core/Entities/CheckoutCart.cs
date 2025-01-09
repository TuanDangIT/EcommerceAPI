using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using Ecommerce.Modules.Carts.Core.Entities.ValueObjects;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class CheckoutCart : BaseEntity, IAuditable
    {
        public Customer Customer { get; private set; } = new();
        public Payment? Payment { get; private set; }
        public Guid? PaymentId { get; private set; }
        public Shipment? Shipment { get; private set; }
        public string? AdditionalInformation { get; private set; }
        public string? StripePaymentIntentId { get; private set; }
        public string? StripeSessionId { get; private set; }
        public bool IsPaid { get; private set; } = false;
        public Cart Cart { get; private set; } = default!;
        public Guid CartId { get; private set; }

        private readonly List<CartProduct> _products = [];
        public IEnumerable<CartProduct> Products => _products;
        public decimal TotalSum { get; private set; }
        public Discount? Discount { get; private set; }
        public int? DiscountId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public CheckoutCart(Cart cart, Guid? customerId)
        {
            Customer.SetCustomerId(customerId ?? Guid.Empty);
            _products = cart.Products.ToList();
            TotalSum = cart.TotalSum;
            Discount = cart.Discount;
            Cart = cart;
        }
        private CheckoutCart()
        {
            
        }
        public void SetCustomer(Customer customer)
            => Customer.SetDetails(customer.FirstName, customer.LastName, customer.Email, customer.PhoneNumber);
        public void FillShipment(Shipment shipment)
        {
            Shipment = shipment;
            SetTotalSum(TotalSum);
        }
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
        public void AddDiscount(Discount discount, decimal totalSum)
        {
            Discount = discount;
            TotalSum = totalSum;
        }
        public void RemoveDiscount(decimal totalSum)
        {
            Discount = null;
            TotalSum = totalSum;
        }
        public void SetTotalSum(decimal totalSum) 
            => TotalSum = totalSum + (Shipment is not null ? Shipment.Price : 0);
    }
}
