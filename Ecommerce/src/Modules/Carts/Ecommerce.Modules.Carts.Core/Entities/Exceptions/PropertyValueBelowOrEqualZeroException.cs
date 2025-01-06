using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.Exceptions
{
    internal class PropertyValueBelowOrEqualZeroException : EcommerceException
    {
        public PropertyValueBelowOrEqualZeroException(string propertyName) : base($"{propertyName}'s value must be higher than 0.")
        {
        }
    }
}
