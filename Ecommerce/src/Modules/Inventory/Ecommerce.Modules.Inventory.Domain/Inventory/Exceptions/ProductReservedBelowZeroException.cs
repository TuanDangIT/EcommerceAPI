﻿using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Ecommerce.Modules.Inventory.Tests.Unit")]
namespace Ecommerce.Modules.Inventory.Domain.Inventory.Exceptions
{
    internal class ProductReservedBelowZeroException : EcommerceException
    {
        public ProductReservedBelowZeroException() : base("Reserved products must be equal or higher than 0.")
        {
        }
    }
}
