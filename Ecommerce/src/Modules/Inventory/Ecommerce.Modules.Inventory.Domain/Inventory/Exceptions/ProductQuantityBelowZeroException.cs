﻿using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductQuantityBelowZeroException : EcommerceException
    {
        public ProductQuantityBelowZeroException() : base("Product's quantity must be higher or equal zero.")
        {
        }
    }
}
