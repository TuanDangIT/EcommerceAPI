using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Pagination.CursorPagination.Exceptions
{
    internal class InvalidValueForEnumTypeException(string filter, Type enumType) : EcommerceException($"The provided value: {filter} is not a valid member of the enum: {enumType.Name}.")
    {
    }
}
