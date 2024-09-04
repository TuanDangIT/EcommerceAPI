using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid? CustomerId {  get; set; }
        private List<CartProduct> _products = [];
        public IEnumerable<CartProduct> Products => _products;
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
        public void AddProduct(Product product, int quantity = 1)
        {
            var cartProduct = _products.SingleOrDefault(p => p.Id == product.Id);
            if(cartProduct is not null)
            {
                cartProduct.IncreaseQuantity(quantity);
                return;
            }
            _products.Add(new CartProduct(product, quantity));
        }
        public void RemoveProduct(Product product)
        {
            var cartProduct = _products.SingleOrDefault(p => p.Id == product.Id);
            if (cartProduct is null)
            {
                throw new CartProductNotFoundException(product.Id);
            }
            if (cartProduct.Quantity == 1)
            {
                _products.Remove(cartProduct);
            }
            else
            {
                cartProduct.DecreaseQuantity();
            }
        }
        public void SetProductQuantity(Product product, int quantity)
        {
            var cartProduct = _products.SingleOrDefault(p => p.Id == product.Id);
            if (cartProduct is null)
            {
                throw new CartProductNotFoundException(product.Id);
            }
            if (cartProduct.Quantity == 0)
            {
                _products.Remove(cartProduct);
            }
            else
            {
                cartProduct.SetQuantity(quantity);
            }
        }
        public void Clear() 
            => _products.Clear();
        public CheckoutCart Checkout()
        {
            if(_products.Count() == 0)
            {
                throw new CartCheckoutWithNoProductsException();
            }
            return new CheckoutCart(this);
        }
    }
}
