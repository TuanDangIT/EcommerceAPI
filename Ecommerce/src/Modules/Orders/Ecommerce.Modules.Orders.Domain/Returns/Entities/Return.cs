using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;
using Ecommerce.Modules.Orders.Domain.Returns.Exception;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Returns.Entities
{
    public class Return : AggregateRoot, IAuditable
    {
        public Order Order { get; private set; } = default!;
        public Guid OrderId { get; private set; }
        private readonly List<ReturnProduct> _products = [];
        public IEnumerable<ReturnProduct> Products => _products;
        public bool AreAllProductsAcceptedOrReturned => _products.All(p => p.Status == ReturnProductStatus.Accepted || p.Status == ReturnProductStatus.Returned);
        public bool HasProducts => _products.Count != 0;
        public string ReasonForReturn { get; private set; } = string.Empty;
        public string? AdditionalNote { get; private set; }
        public string? RejectReason { get; private set; }
        public ReturnStatus Status { get; private set; } = ReturnStatus.NotHandled;
        public bool IsFullReturn { get; private set; }
        public bool IsCompleted => Status is ReturnStatus.Handled;
        public decimal TotalSum => _products.Sum(p => p.Price);
        public decimal TotalSumLeftToReturn => _products.Where(p => p.Status != ReturnProductStatus.Returned).Sum(p => p.Price);
        public bool HasReturned => _products.Any(p => p.Status == ReturnProductStatus.Returned);
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Return(Guid id, Order order, IEnumerable<ReturnProduct> products, string reasonForReturn, 
            bool isFullReturn)
        {
            Id = id;
            Order = order;
            _products = products.ToList();
            ReasonForReturn = reasonForReturn;
            IsFullReturn = isFullReturn;
        }
        private Return()
        {
            
        }
        public void ChangeStatus(ReturnStatus status)
        {
            Status = status;
        }
        public void Handle()
        {
            foreach (var product in _products)
            {
                product.SetStatus(ReturnProductStatus.Returned);
            }
            ChangeStatus(ReturnStatus.Handled);
            IncrementVersion();
        }
        public void SetNote(string note)
        {
            AdditionalNote = note;
            IncrementVersion();
        }
        public void Reject(string rejectReason)
        {
            RejectReason = rejectReason;    
            ChangeStatus(ReturnStatus.Rejected);
            IncrementVersion();
        }
        public void SetProductStatus(int productId, ReturnProductStatus status)
        {
            var product = GetProductOrThrow(productId, Id);
            if (product.Status == ReturnProductStatus.Returned)
            {
                throw new NotSupportedException("Cannot set return product status to diffrent one when the currect one is returned. The operation is currently not supported");
            }
            product.SetStatus(status);
            IncrementVersion();
        }
        public void AddProduct(string sku, int quantity)
        {
            var returnProduct = _products.SingleOrDefault(p => p.SKU == sku);
            if(returnProduct is not null)
            {
                if(returnProduct.Status == ReturnProductStatus.Returned)
                {
                    throw new NotSupportedException("Cannot add product to return product that status is returned. The operation is currently not supported");
                }
                SetProductQuantity(returnProduct.Id, returnProduct.Quantity + quantity);
                return;
            }
            var orderItem = Order.Products.SingleOrDefault(p => p.SKU == sku) ??
                throw new ProductNotFoundException(sku);
            if(orderItem.Quantity < quantity)
            {
                throw new ReturnProductQuantityExceedLimitException(quantity, orderItem.Quantity);
            }
            _products.Add(new ReturnProduct(sku, orderItem.Name, orderItem.Price, orderItem.UnitPrice, quantity, orderItem.ImagePathUrl));

        }
        public void AddProduct(ReturnProduct product)
        {
            var existingProduct = _products.SingleOrDefault(p => p.SKU == product.SKU);
            if(existingProduct is not null)
            {
                if (existingProduct.Status == ReturnProductStatus.Returned)
                {
                    throw new NotSupportedException("Cannot add product to return product that status is returned. The operation is currently not supported");
                }
                existingProduct.IncreaseQuantity(product.Quantity);
            }
            else
            {
                _products.Add(product);
            }
            IncrementVersion();
        }
        public void RemoveProduct(int productId)
        {
            var product = GetProductOrThrow(productId, Id);
            if(product.Status == ReturnProductStatus.Returned)
            {
                throw new CannotRemoveAlreadyReturnedReturnProductException(productId);
            }
            _products.Remove(product);
            if (IsFullReturn is true)
            {
                IsFullReturn = false;
            }
            IncrementVersion();
        }
        public void SetProductQuantity(int productId, int quantity)
        {
            if(quantity < 0)
            {
                throw new ReturnInvalidSetQuantityException();
            }
            var returnProduct = GetProductOrThrow(productId, Id);
            if(returnProduct.Status == ReturnProductStatus.Returned)
            {
                throw new CannotSetQuantityForAlreadyReturnedReturnProductException(productId);
            }
            var orderProduct = Order.Products.SingleOrDefault(p => p.SKU == returnProduct.SKU) ??
                throw new ProductNotFoundException(returnProduct.SKU);
            var limit = orderProduct.Quantity + returnProduct.Quantity;
            if(quantity > limit)
            {
                throw new ReturnProductQuantityExceedLimitException(quantity, limit);
            }
            if (quantity == 0)
            {
                RemoveProduct(productId);
            }
            returnProduct.SetQuantity(quantity);
            ChangeFullReturnFlag();
            IncrementVersion();
        }
        public ReturnProduct? GetReturnProduct(int productId)
            => _products.SingleOrDefault(p => p.Id == productId);
        private ReturnProduct GetProductOrThrow(int productId, Guid returnId)
            => _products.SingleOrDefault(p => p.Id == productId) ??
                throw new ReturnProductNotFoundException(productId, returnId);
        private void ChangeFullReturnFlag()
        {
            if(Order.Products.Any(p => p.Price != 0 || p.Quantity != 0))
            {
                IsFullReturn = false;
                return;
            }
            IsFullReturn = true;
        }
    }
}
