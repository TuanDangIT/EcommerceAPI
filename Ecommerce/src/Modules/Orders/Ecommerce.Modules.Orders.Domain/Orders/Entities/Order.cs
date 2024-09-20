using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
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
        public Guid CustomerId { get; set; }
        private readonly List<Product> _products = [];
        public IEnumerable<Product> Products => _products;
        public Shipment Shipment { get; set; } = new();
        public PaymentMethod Payment { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Placed;
        public bool IsCompleted => Status is OrderStatus.Cancelled || Status is OrderStatus.Completed || Status is OrderStatus.Returned;
        public string? AdditionalInformation { get; set; }
        public DateTime OrderPlacedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
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
        public void ChangeStatus(OrderStatus status, DateTime updatedAt)
        {
            Status = status;
            UpdatedAt = updatedAt;
        }
        public void EditShipment(Shipment shipment, DateTime updatedAt)
        {
            Shipment = shipment;
            UpdatedAt = updatedAt;
        }
        public void CancelOrder(DateTime updatedAt)
        {
            Status = OrderStatus.Cancelled;
            UpdatedAt = updatedAt;
        }
        public void ShipOrder(DateTime updatedAt)
        {
            Status = OrderStatus.Shipped;
            UpdatedAt = updatedAt;
        }
        public void PackOrder(DateTime updatedAt)
        {
            Status = OrderStatus.ParcelPacked;
            UpdatedAt = updatedAt;
        }
        public void ReturnOrder(DateTime updatedAt)
        {
            Status = OrderStatus.Returned;
            UpdatedAt = updatedAt;
        }
        public void CompleteOrder(DateTime updatedAt)
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
    }
}
