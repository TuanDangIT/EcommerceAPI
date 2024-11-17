using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Order : AggregateRoot, IAuditable
    {
        public Customer Customer { get; private set; } = new();
        private readonly List<Product> _products = [];
        public IEnumerable<Product> Products => _products;
        public decimal TotalSum { get; private set; }
        public PaymentMethod Payment { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Placed;
        public bool IsCompleted => Status is OrderStatus.Cancelled || Status is OrderStatus.Completed || Status is OrderStatus.Returned;
        public string? ClientAdditionalInformation { get; private set; }
        public string? CompanyAdditionalInformation { get; private set; }
        public string? DiscountCode { get; private set; }
        public string StripePaymentIntentId { get; private set; } = string.Empty;
        private readonly List<Shipment> _shipments = [];
        public IEnumerable<Shipment> Shipments => _shipments;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Invoice? Invoice { get; private set; } 
        public Order(Guid id, Customer customer, IEnumerable<Product> products, decimal totalSum, PaymentMethod paymentMethod, 
            DateTime createdAt, string? clientAdditionalInformation, string? discountCode, string stripePaymentIntentId)
        {
            Id = id;
            Customer = customer;
            _products = products.ToList();
            Payment = paymentMethod;
            CreatedAt = createdAt;
            ClientAdditionalInformation = clientAdditionalInformation;
            DiscountCode = discountCode;
            StripePaymentIntentId = stripePaymentIntentId;
            TotalSum = totalSum;
        }
        public Order()
        {
            
        }
        public void AddShipment(Shipment shipment)
        {
            _shipments.Add(shipment);
            IncrementVersion();
        }
        public void DeleteShipment(int shipmentId)
        {
            var shipment = _shipments.SingleOrDefault(s => s.Id == shipmentId);
            if(shipment is null)
            {
                throw new ShipmentNotFoundException(shipmentId);
            }
            _shipments.Remove(shipment);
            IncrementVersion();
        }
        public void ChangeStatus(OrderStatus status)
        {
            Status = status;
            IncrementVersion();
        }
        public void Cancel()
        {
            ChangeStatus(OrderStatus.Cancelled);
            IncrementVersion();
        }
        public void Ship()
        {
            ChangeStatus(OrderStatus.Shipped);
            IncrementVersion();
        }
        public void Pack()
        {
            ChangeStatus(OrderStatus.ParcelPacked);
            IncrementVersion();
        }
        public void Return()
        {
            ChangeStatus(OrderStatus.Returned);
            IncrementVersion();
        }
        public void Complete()
        {
            ChangeStatus(OrderStatus.Completed);
            IncrementVersion();
        }
        public void DecreaseProductQuantity(string sku, int quantity)
        {
            var product = _products.SingleOrDefault(p => p.SKU == sku);
            if(product is null)
            {
                throw new ProductNotFoundException(sku);
            }
            if(product.Quantity == quantity || product.Quantity == 1)
            {
                _products.Remove(product);
            }
            else
            {
                product.DecreaseQuantity(quantity); 
            }
            IncrementVersion();
        }
        public void SetCompanyAdditionalInformation(string companyAdditionalInformation)
        {
            CompanyAdditionalInformation = companyAdditionalInformation;
            IncrementVersion();
        }
        public void EditCustomer(Customer customer)
        {
            Customer = customer;
            IncrementVersion();
        }
        public void WriteAdditionalInformation(string additionalInformation)
        {
            CompanyAdditionalInformation = additionalInformation;
            IncrementVersion();
        }
    }
}
