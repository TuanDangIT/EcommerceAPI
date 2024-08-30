using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    internal class Cart
    {
        public Guid Id { get; set; }
        public Guid CustomerId {  get; set; }
        public List<CartProduct> _products = new();
        public IEnumerable<CartProduct> Products => _products;
        public Cart(Guid customerId)
        {
            CustomerId = customerId;
        }
        public void AddProduct(Product product)
        {
            var cartProduct = _products.SingleOrDefault(p => p.Id == product.Id);
            if(cartProduct is not null)
            {
                cartProduct.DecreaseQuantity();
            }
            _products.Add(new CartProduct(product, 1));
        }
        public void RemoveProduct(Product product)
        {
            var cartProduct = _products.SingleOrDefault(p => p.Id == product.Id);
            if( cartProduct is null)
            {
                throw new CartProductNotFoundException(product.Id);
            }
            if(cartProduct.Quantity == 1)
            {
                _products.Remove(cartProduct);
            }
            else
            {
                cartProduct.DecreaseQuantity();
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
