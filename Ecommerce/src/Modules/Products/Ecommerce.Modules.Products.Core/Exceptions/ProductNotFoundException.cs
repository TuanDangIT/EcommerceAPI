﻿using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Exceptions
{
    public class ProductNotFoundException : EcommerceException
    {
        public Guid Id { get; }
        public ProductNotFoundException(Guid id) : base($"Product: {id} was not found.")
        {
            Id = id;
        }
    }
}
