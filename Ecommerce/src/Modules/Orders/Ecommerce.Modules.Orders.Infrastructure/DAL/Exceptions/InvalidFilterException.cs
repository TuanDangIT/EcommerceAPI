﻿using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.DAL.Exceptions
{
    public class InvalidFilterException(string filter) : EcommerceException($"Given filter: {filter} is wrong.")
    {
    }
}
