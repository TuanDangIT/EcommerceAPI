using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
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
            var cartProduct = _products.SingleOrDefault(p => p.ProductId == product.Id);
            if (cartProduct is not null)
            {
                cartProduct.IncreaseQuantity(quantity);
            }
            else
            {
                _products.Add(new CartProduct(product, quantity));
            }
            TotalSum = CalculateTotalSum();
            CheckoutCart?.SetTotalSum(TotalSum);
        }
        public void RemoveProduct(Product product, int quantity)
        {
            var cartProduct = _products.SingleOrDefault(cp => cp.ProductId == product.Id) ?? 
                throw new CartProductNotFoundException(product.Id);
            if (cartProduct.Quantity == quantity || cartProduct.Quantity == 1)
            {
                cartProduct.DecreaseQuantity(quantity);
                _products.Remove(cartProduct);
            }
            else
            {
                cartProduct.DecreaseQuantity(quantity);
            }
            TotalSum = CalculateTotalSum();
            CheckoutCart?.SetTotalSum(TotalSum);
        }
        public (bool? IsReservationRequired, int Diffrence) SetProductQuantity(Product product, int quantity)
        {
            var cartProduct = _products.SingleOrDefault(cp => cp.ProductId == product.Id) ??
                throw new CartProductNotFoundException(product.Id);
            bool isReservationRequired;
            var diffrence = Math.Abs(cartProduct.Quantity - quantity);
            if (cartProduct.Quantity == quantity)
            {
                return default!;
            }
            if (quantity == 0)
            {
                cartProduct.SetQuantity(quantity);
                _products.Remove(cartProduct);
                isReservationRequired = false;
            }
            else
            {
                if(cartProduct.Quantity < quantity)
                {
                    isReservationRequired = true;
                }
                else
                {
                    isReservationRequired = false;
                }
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
            var checkoutCart = new CheckoutCart(this, customerId);
            CheckoutCart = checkoutCart;
            return checkoutCart;
        }
        public void AddDiscount(Discount discount)
        {
            Discount = discount;
            if (discount.SKU is not null)
            {
                var cartProduct = _products.SingleOrDefault(p => p.Product.SKU == discount.SKU);
                if (cartProduct is null)
                {
                    Discount = null;
                    throw new CheckoutCartCannotApplyIndividualDiscountException(discount.SKU);
                }
                cartProduct.ApplyDiscountedPrice(Discount.Value);
            }
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
            decimal calculatedTotal = productsTotal;
            if (Discount.Type is DiscountType.NominalDiscount)
            {
                if (Discount.SKU is not null)
                {
                    var discountedProductQuantity = _products
                        .Where(p => p.Product.SKU == Discount.SKU)
                        .Sum(p => p.Quantity);
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
