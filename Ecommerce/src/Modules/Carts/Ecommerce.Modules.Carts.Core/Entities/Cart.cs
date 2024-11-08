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
    public class Cart : BaseEntity
    {
        public Guid? CustomerId {  get; private set; }
        private List<CartProduct> _products = [];
        public IEnumerable<CartProduct> Products => _products;
        public decimal TotalSum => _products.Sum(cp => cp.Product.Price*cp.Quantity);
        public Cart(Guid id, Guid customerId)
        {
            CustomerId = customerId;
        }
        public Cart(Guid id)
        {
            
        }
        public Cart()
        {
            
        }
        public void AddProduct(Product product, int quantity)
        {
            var cartProduct = _products.SingleOrDefault(p => p.ProductId == product.Id);
            if(cartProduct is not null)
            {
                cartProduct.IncreaseQuantity(quantity);
                return;
            }
            _products.Add(new CartProduct(product, quantity));
        }
        public void RemoveProduct(Product product, int quantity)
        {
            var cartProduct = _products.SingleOrDefault(cp => cp.ProductId == product.Id) ?? throw new CartProductNotFoundException(product.Id);
            if (cartProduct.Quantity == quantity || cartProduct.Quantity == 1)
            {
                cartProduct.DecreaseQuantity(quantity);
                _products.Remove(cartProduct);
            }
            else
            {
                cartProduct.DecreaseQuantity(quantity);
            }
        }
        public void SetProductQuantity(Product product, int quantity)
        {

            var cartProduct = _products.SingleOrDefault(cp => cp.ProductId == product.Id) ?? throw new CartProductNotFoundException(product.Id);
            if (quantity == 0)
            {
                cartProduct.SetQuantity(quantity);
                _products.Remove(cartProduct);
            }
            else
            {
                cartProduct.SetQuantity(quantity);
            }
        }
        public void Clear()
        {
            foreach(var cartProduct in _products)
            {
                cartProduct.SetQuantity(0);
            }
            _products.Clear();
        }
        public CheckoutCart Checkout()
        {
            if (_products.Count == 0)
            {
                throw new CartCheckoutWithNoProductsException();
            }
            return new CheckoutCart(this);
        }
    }
}
