using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
            product.DecreaseQuantity(quantity);
        }
        public CartProduct()
        {
            
        }
        public void IncreaseQuantity(int quantity)
        {
            if (Product.HasQuantity)
            {
                Product.DecreaseQuantity(quantity);
            }
            Quantity += quantity;
        }
        public void DecreaseQuantity(int quantity)
        {
            if(Quantity - quantity < 0)
            {
                throw new CartProductQuantityBelowZeroException();
            }
            if (Product.HasQuantity)
            {
                Product.IncreaseQuantity(quantity);
            }
            Quantity -= quantity;
        }
        public void SetQuantity(int quantity)
        {
            if (Product.HasQuantity)
            {
                if (quantity > Quantity)
                {
                    Product.DecreaseQuantity(quantity - Quantity);
                }
                else if (quantity < Quantity)
                {
                    Product.IncreaseQuantity(Quantity - quantity);
                }
            }
            Quantity = quantity;
        }
    }
}
