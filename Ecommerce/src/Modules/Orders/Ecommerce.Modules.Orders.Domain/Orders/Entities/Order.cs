﻿using Azure.Core;
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
        public Customer? Customer { get; private set; } = default!;
        private readonly List<Product> _products = [];
        public IEnumerable<Product> Products => _products;
        public decimal TotalSum { get; private set; }
        public PaymentMethod Payment { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Placed;
        public bool IsCompleted => Status is OrderStatus.Cancelled || Status is OrderStatus.Completed || Status is OrderStatus.Returned;
        public string? ClientAdditionalInformation { get; private set; }
        public string? CompanyAdditionalInformation { get; private set; }
        public Discount? Discount { get; private set; }
        public string StripePaymentIntentId { get; private set; } = string.Empty;
        public DeliveryService DeliveryService { get; private set; } = default!;
        private readonly List<Shipment> _shipments = [];
        public IEnumerable<Shipment> Shipments => _shipments;
        public Return? Return { get; private set; }
        private readonly List<Complaint>? _complaints = [];
        public IEnumerable<Complaint>? Complaints => _complaints;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Invoice? Invoice { get; private set; }
        public bool HasInvoice => Invoice is not null;
        public Order(Guid id, Customer customer, IEnumerable<Product> products, decimal totalSum, PaymentMethod paymentMethod, 
            DeliveryService deliveryService, string? clientAdditionalInformation, Discount? discount, string stripePaymentIntentId)
        {
            if(totalSum < 0)
            {
                throw new OrderTotalSumBelowZeroException();
            }
            if(discount?.Value <= 0)
            {
                throw new OrderDiscountValueBelowOrEqualZeroException();
            }
            Id = id;
            Customer = customer;
            _products = products.ToList();
            Payment = paymentMethod;
            DeliveryService = deliveryService;
            ClientAdditionalInformation = clientAdditionalInformation;
            Discount = discount;
            StripePaymentIntentId = stripePaymentIntentId;
            TotalSum = totalSum;
        }
        private Order()
        {
            
        }
        public static Order CreateDraft(Guid orderId)
            => new()
            {
                Id = orderId,
                Status = OrderStatus.Draft
            };
        public void Submit()
        {
            if(Status != OrderStatus.Draft)
            {
                throw new OrderCannotSubmitException();
            }
            ChangeStatus(OrderStatus.Placed);
        }
        public void AddShipment(Shipment shipment)
        {
            _shipments.Add(shipment);
            IncrementVersion();
        }
        public void DeleteShipment(Shipment shipment)
        {
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
        public void ReturnProducts(IEnumerable<(int ProductId, int Quantity)> products)
        {
            foreach (var (productId, quantity) in products)
            {
                var product = FindProductById(productId);
                if (product.Quantity < quantity)
                {
                    throw new CannotReturnMoreProductsThanBoughtException(productId, product.Quantity, quantity);
                }
                product.DecreaseQuantity(quantity);
            }
            ChangeStatus(OrderStatus.Returned);
            CalculateTotalSum();
            IncrementVersion();
        }
        public void Complete()
        {
            ChangeStatus(OrderStatus.Completed);
            IncrementVersion();
        }
        public void AddProduct(Product product)
        {
            _products.Add(product);
            CalculateTotalSum();
            IncrementVersion();
        }
        public void AddProduct(int productId, int quantity)
        {
            var product = FindProductById(productId);
            product.IncreaseQuantity(quantity);
            CalculateTotalSum();
            IncrementVersion();
        }
        public void AddProduct(string sku, int quantity)
        {
            var product = FindProductBySku(sku);
            product.IncreaseQuantity(quantity);
            CalculateTotalSum();
            IncrementVersion();
        }
        public void RemoveProduct(int productId, int? quantity)
        {
            var product = FindProductById(productId);
            RemoveProduct(product, quantity);
        }
        //public void RemoveProduct(string sku, int quantity)
        //{
        //    var product = FindProductBySku(sku);
        //    product.DecreaseQuantity(quantity);
        //}
        public void RemoveProduct(Product product, int? quantity)
        {
            if (quantity is null || product.Quantity == quantity || product.Quantity == 1)
            {
                _products.Remove(product);
            }
            else
            {
                product.DecreaseQuantity((int)quantity);
            }
            CalculateTotalSum();
            IncrementVersion();
        }
        public void DecreaseProductQuantity(string sku, int quantity)
        {
            var product = FindProductBySku(sku);
            product.DecreaseQuantity(quantity);
            CalculateTotalSum();
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

        private void CalculateTotalSum()
        {
            var productsTotal = _products.Sum(p => p.Price) + DeliveryService.Price;
            if (Discount is null)
            {
                TotalSum = productsTotal;
                return;
            }
            decimal calculatedTotal = productsTotal;
            if (Discount.Type is DiscountType.NominalDiscount)
            {
                if (!string.IsNullOrEmpty(Discount.SKU))
                {
                    var discountedProduct = _products
                        .SingleOrDefault(p => p.SKU == Discount.SKU) ?? throw new ProductNotFoundException(Discount.SKU);
                    var discountedProductQuantity = discountedProduct.Quantity;
                    calculatedTotal = productsTotal - Discount.Value * discountedProductQuantity;
                }
                else
                {
                    calculatedTotal = productsTotal - Discount.Value;
                }
            }
            else
            {
                calculatedTotal = productsTotal * Discount.Value;
            }
            if (calculatedTotal <= 0)
            {
                TotalSum = 0 + DeliveryService.Price;
                return;
            }
            TotalSum = calculatedTotal;
        }
        private Product FindProductById(int productId) =>
            _products.FirstOrDefault(p => p.Id == productId) ??
            throw new ProductNotFoundException(productId);

        private Product FindProductBySku(string sku) =>
            _products.SingleOrDefault(p => p.SKU == sku) ??
            throw new ProductNotFoundException(sku);
    }
}
