using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    internal class CartProduct
    {
        public Guid Id { get; set; }
        public Product Product { get; set; } = new();
        public int Quantity { get; set; }
        public CartProduct(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
        public void IncreaseQuantity() => Quantity++;
        public void DecreaseQuantity()
        {
            if(Quantity -1 < 0)
            {
                throw new CartProductQuantityBelowZeroException();
            }
            Quantity--;
        }
    }
}
