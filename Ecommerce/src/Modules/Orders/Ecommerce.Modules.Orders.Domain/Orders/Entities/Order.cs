using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Invoices.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Modules.Orders.Domain.Shipping.Entities;
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
        //public Guid CustomerId { get; set; }
        private readonly List<Product> _products = [];
        public IEnumerable<Product> Products => _products;
        public decimal TotalSum { get; private set; }
        //public ShipmentDetails ShipmentDetails { get; set; } = new();
        public PaymentMethod Payment { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Placed;
        public bool IsCompleted => Status is OrderStatus.Cancelled || Status is OrderStatus.Completed || Status is OrderStatus.Returned;
        public string? ClientAdditionalInformation { get; private set; }
        public string? CompanyAdditionalInformation { get; private set; }
        public string? DiscountCode { get; private set; }
        public string StripePaymentIntentId { get; private set; } = string.Empty;
        public Shipment Shipment { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Invoice? Invoice { get; private set; } 
        //private readonly List<Complaint> _complaints = [];
        //public IEnumerable<Complaint> Complaints => _complaints;
        //public Return? ReturnOrder { get; set; }
        public Order(Guid id, Customer customer, IEnumerable<Product> products, decimal totalSum, /*ShipmentDetails shipmentDetails,*/Shipment shipment,
            PaymentMethod paymentMethod, DateTime createdAt, string? clientAdditionalInformation, string? discountCode, string stripePaymentIntentId)
        {
            Id = id;
            Customer = customer;
            _products = products.ToList();
            Shipment = shipment;
            //ShipmentDetails = shipmentDetails;
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
        public void ChangeStatus(OrderStatus status)
        {
            Status = status;
            IncrementVersion();
        }
        //public void EditShipment(ShipmentDetails shipmentDetails, DateTime updatedAt)
        //{
        //    ShipmentDetails = shipmentDetails;
        //    UpdatedAt = updatedAt;
        //}
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
        public void SetParcels(IEnumerable<Parcel> parcels)
        {
            Shipment.SetParcels(parcels);
            IncrementVersion();
        }
        public void SetLabelDetails(string trackingNumber, string labelId, DateTime labelCreatedAt)
        {
            Shipment.SetLabelId(labelId);
            Shipment.SetTrackingNumber(trackingNumber);
            Shipment.SetLabelCreatedAt(labelCreatedAt);
            IncrementVersion();
        }
        public void SetCompanyAdditionalInformation(string companyAdditionalInformation)
        {
            CompanyAdditionalInformation = companyAdditionalInformation;
            IncrementVersion();
        }
        
    }
}
