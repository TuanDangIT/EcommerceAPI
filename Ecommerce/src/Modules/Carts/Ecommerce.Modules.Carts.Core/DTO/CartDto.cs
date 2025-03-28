﻿using Ecommerce.Modules.Carts.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.DTO
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public decimal TotalSum { get; set; }
        public IEnumerable<CartProductDto> Products { get; set; } = [];
        public DiscountDto? Discount { get; set; } = new();
    }
}
