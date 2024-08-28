using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Exceptions
{
    internal class ParameterNotDeletedException : EcommerceException
    {
        public Guid Id { get; }
        public ParameterNotDeletedException(Guid id) : base($"Parameter: {id} was not deleted.")
        {
            Id = id;
        }
    }
}
