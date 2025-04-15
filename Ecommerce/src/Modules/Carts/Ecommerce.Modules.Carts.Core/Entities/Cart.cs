using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class Cart : BaseEntity, IAuditable
    {
        private readonly List<CartProduct> _products = [];
        public IEnumerable<CartProduct> Products => _products;
        public decimal TotalSum { get; private set; }
        public Discount? Discount { get; private set; }
        public int? DiscountId { get; private set; }
        public CheckoutCart? CheckoutCart { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool HasDiscount => Discount is not null;
        public Cart()
        {
            
        }
        public void AddProduct(Product product, int quantity)
        {
            if(product.Quantity < quantity)
            {
                throw new ProductOutOfStockException(product.Id);
            }
            var cartProduct = _products.SingleOrDefault(p => p.Product.Id == product.Id);
            if (cartProduct is not null)
            {
                cartProduct.IncreaseQuantity(quantity);
            }
            else
            {
                _products.Add(new CartProduct(product, quantity, CheckoutCart));
            }
            TotalSum = CalculateTotalSum();
            CheckoutCart?.SetTotalSum(TotalSum);
        }
        public void RemoveProduct(Product product, int quantity)
        {
            var cartProduct = _products.SingleOrDefault(cp => cp.ProductId == product.Id) ?? 
                throw new CartProductNotFoundException(product.Id);
            if (cartProduct.Quantity <= quantity)
            {
                cartProduct.DecreaseQuantity(quantity);
                _products.Remove(cartProduct);
                if(!string.IsNullOrEmpty(Discount?.SKU) && Discount?.SKU == product.SKU)
                {
                    RemoveDiscount();
                }
            }
            else
            {
                cartProduct.DecreaseQuantity(quantity);
            }
            TotalSum = CalculateTotalSum();
            CheckoutCart?.SetTotalSum(TotalSum);
        }
        public (bool? IsReservationRequired, int Difference) SetProductQuantity(Product product, int quantity)
        {
            if (quantity < 0)
            {
                throw new SetQuantityBelowZeroException();
            }
            var cartProduct = _products.SingleOrDefault(cp => cp.ProductId == product.Id) ??
                throw new CartProductNotFoundException(product.Id, Id);
            if (cartProduct.Quantity == quantity)
            {
                return default!;
            }
            var diffrence = Math.Abs(cartProduct.Quantity - quantity);
            bool? isReservationRequired = null;
            if (quantity == 0)
            {
                cartProduct.SetQuantity(0);
                _products.Remove(cartProduct);
                isReservationRequired = false;
            }
            else
            {
                isReservationRequired = cartProduct.Quantity < quantity;
                cartProduct.SetQuantity(quantity);
            }
            TotalSum = CalculateTotalSum();
            CheckoutCart?.SetTotalSum(TotalSum);
            return (isReservationRequired, diffrence);
        }
        public void Clear()
        {
            foreach(var cartProduct in _products)
            {
                cartProduct.SetQuantity(0);
            }
            _products.Clear();
            Discount = null;
            TotalSum = 0;
            CheckoutCart?.SetTotalSum(TotalSum);
            CheckoutCart?.RemoveDiscount();
        }
        public CheckoutCart Checkout(Guid? customerId)
        {
            if (_products.Count == 0)
            {
                throw new CartCheckoutWithNoProductsException();
            }
            if(CheckoutCart is not null)
            {
                return CheckoutCart;
            }
            return CheckoutCart ??= new CheckoutCart(this, customerId);
        }
        public void AddDiscount(Discount discount)
        {
            if(Discount is not null)
            {
                throw new CartCannotHaveMoreThanOneDiscountException(Id);
            }
            if(_products.Count == 0)
            {
                throw new AddDiscountToEmptyCartException();
            }
            if(discount.RequiredCartTotalValue > TotalSum)
            {
                throw new InsufficientCartTotalForDiscountException(discount.RequiredCartTotalValue, TotalSum);
            }
            if(discount.Type == DiscountType.NominalDiscount && discount.Value >= TotalSum)
            {
                throw new AddDiscountHigherThanTotalValueException(discount.Id, TotalSum);
            }
            if (!string.IsNullOrEmpty(discount.SKU))
            {
                var cartProduct = _products.SingleOrDefault(p => p.Product.SKU == discount.SKU) 
                    ?? throw new CheckoutCartCannotApplyIndividualDiscountException(discount.SKU);
                cartProduct.ApplyDiscountedPrice(discount.Value);
            }
            Discount = discount;
            TotalSum = CalculateTotalSum();
            CheckoutCart?.AddDiscount(discount, TotalSum);
        }
        public void RemoveDiscount()
        {
            Discount = null;
            TotalSum = CalculateTotalSum();
            CheckoutCart?.RemoveDiscount(TotalSum);
        }
        private decimal CalculateTotalSum()
        {
            var productsTotal = _products.Sum(cp => cp.Product.Price * cp.Quantity);
            if (Discount is null)
            {
                return productsTotal;
            }
            if (productsTotal == 0)
            {
                Discount = null;
                CheckoutCart?.RemoveDiscount(productsTotal);
                return productsTotal;
            }
            decimal calculatedTotal = productsTotal;
            if (Discount.Type is DiscountType.NominalDiscount)
            {
                if (!string.IsNullOrEmpty(Discount.SKU))
                {
                    var discountedProduct = _products
                        .SingleOrDefault(p => p.Product.SKU == Discount.SKU) ?? throw new ProductNotFoundException(Discount.SKU);
                    var discountedProductQuantity =  discountedProduct.Quantity;
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
                throw new CartCalculatedTotalBelowOrEqualZeroException();
            }

            return calculatedTotal;
        }
    }
}
