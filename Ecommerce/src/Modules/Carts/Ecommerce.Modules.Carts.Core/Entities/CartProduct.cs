using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class CartProduct
    {
        public Guid Id { get; private set; }
        public Product Product { get; private set; } = new();
        public Guid ProductId { get; private set; }
        public Cart Cart { get; private set; } = new();
        public Guid CartId { get; private set; }
        public CheckoutCart? CheckoutCart { get; private set; }
        public Guid? CheckoutCartId { get; private set; }
        public int Quantity { get; private set; }
        public CartProduct(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
        public CartProduct()
        {
            
        }
        public void IncreaseQuantity(int quantity) => Quantity+=quantity;
        public void DecreaseQuantity()
        {
            if(Quantity -1 < 0)
            {
                throw new CartProductQuantityBelowZeroException();
            }
            Quantity--;
        }
        public void SetQuantity(int quantity) => Quantity = quantity;
    }
}
