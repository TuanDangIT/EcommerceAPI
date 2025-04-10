﻿using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Exceptions
{
    public class InvalidFilterException(string filter) : EcommerceException($"Filtering is not supported for type {filter}.")
    {
    }
}
