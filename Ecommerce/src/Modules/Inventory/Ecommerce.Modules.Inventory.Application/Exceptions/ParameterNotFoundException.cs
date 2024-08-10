using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Exceptions
{
    internal class ParameterNotFoundException : EcommerceException
    {
        public Guid Id { get; }
        public ParameterNotFoundException(Guid id) : base($"Parameter: {id} was not found.")
        {
            Id = id;
        }
    }
}
