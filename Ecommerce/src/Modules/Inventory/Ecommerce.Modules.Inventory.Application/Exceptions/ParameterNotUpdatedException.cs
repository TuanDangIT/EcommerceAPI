﻿using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Exceptions
{
    internal class ParameterNotUpdatedException : EcommerceException
    {
        public Guid Id { get; }
        public ParameterNotUpdatedException(Guid id) : base($"Parameter: {id} was not updated.")
        {
            Id = id;
        }
    }
}
