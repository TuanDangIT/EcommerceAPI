using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Modules.Orders.Domain.Shipping.Entities;
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
        //public Guid CustomerId { get; set; }
        private readonly List<Product> _products = [];
        public IEnumerable<Product> Products => _products;
        public decimal TotalSum { get; set; }
        //public ShipmentDetails ShipmentDetails { get; set; } = new();
        public PaymentMethod Payment { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Placed;
        public bool IsCompleted => Status is OrderStatus.Cancelled || Status is OrderStatus.Completed || Status is OrderStatus.Returned;
        public string? AdditionalInformation { get; set; }
        public string? DiscountCode { get; set; }
        public string StripePaymentIntentId { get; set; } = string.Empty;
        public Shipment Shipment { get; set; } = default!;
        public DateTime OrderPlacedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        //private readonly List<Complaint> _complaints = [];
        //public IEnumerable<Complaint> Complaints => _complaints;
        //public Return? ReturnOrder { get; set; }
        public Order(Guid id, Customer customer, IEnumerable<Product> products, decimal totalSum, /*ShipmentDetails shipmentDetails,*/Shipment shipment,
            PaymentMethod paymentMethod, DateTime orderPlacedAt, string? additionalInformation, string? discountCode, string stripePaymentIntentId)
        {
            Id = id;
            Customer = customer;
            _products = products.ToList();
            Shipment = shipment;
            //ShipmentDetails = shipmentDetails;
            Payment = paymentMethod;
            OrderPlacedAt = orderPlacedAt;
            AdditionalInformation = additionalInformation;
            DiscountCode = discountCode;
            StripePaymentIntentId = stripePaymentIntentId;
        }
        public Order()
        {
            
        }
        public void ChangeStatus(OrderStatus status, DateTime updatedAt)
        {
            Status = status;
            UpdatedAt = updatedAt;
        }
        //public void EditShipment(ShipmentDetails shipmentDetails, DateTime updatedAt)
        //{
        //    ShipmentDetails = shipmentDetails;
        //    UpdatedAt = updatedAt;
        //}
        public void Cancel(DateTime updatedAt)
        {
            Status = OrderStatus.Cancelled;
            UpdatedAt = updatedAt;
        }
        public void Ship(DateTime updatedAt)
        {
            Status = OrderStatus.Shipped;
            UpdatedAt = updatedAt;
        }
        public void Pack(DateTime updatedAt)
        {
            Status = OrderStatus.ParcelPacked;
            UpdatedAt = updatedAt;
        }
        public void Return(DateTime updatedAt)
        {
            Status = OrderStatus.Returned;
            UpdatedAt = updatedAt;
        }
        public void Complete(DateTime updatedAt)
        {
            Status = OrderStatus.Completed;
            UpdatedAt = updatedAt;
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
        }
        public void SetParcels(IEnumerable<Parcel> parcels, DateTime updatedAt)
        {
            UpdatedAt = updatedAt;
            Shipment.SetParcels(parcels);
        }
        public void SetLabelDetails(string trackingNumber, string labelId, DateTime updatedAt)
        {
            UpdatedAt = updatedAt;
            Shipment.SetLabelId(labelId);
            Shipment.SetTrackingNumber(trackingNumber); 
        }
        
    }
}
