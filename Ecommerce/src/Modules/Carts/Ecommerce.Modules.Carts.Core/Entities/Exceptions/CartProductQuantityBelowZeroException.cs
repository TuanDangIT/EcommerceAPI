﻿using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Ecommerce.Modules.Carts.Tests.Unit")]
namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class CartProductQuantityBelowZeroException : EcommerceException
    {
        public CartProductQuantityBelowZeroException() : base("Cart product's quantity must be higher or equal 0.")
        {
        }
    }
}
