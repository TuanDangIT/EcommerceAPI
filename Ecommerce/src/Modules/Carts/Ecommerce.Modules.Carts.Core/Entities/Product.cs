﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    internal class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImagePathUrl { get; set; } = string.Empty;
        private List<CartProduct> _cartProducts { get; set; } = new();
        public IEnumerable<CartProduct> CartProducts => _cartProducts;
        public Product(string name, decimal price, string imagePathUrl)
        {
            Name = name;
            Price = price;
            ImagePathUrl = imagePathUrl;
        }
        public Product()
        {
            
        }
    }
}
