using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Exceptions
{
    internal sealed class ManufacturerNotFoundException : EcommerceException
    {
        public Guid Id { get; }
        public ManufacturerNotFoundException(Guid id) : base($"Manufacturer: {id} was not found.")
        {
            Id = id;
        }
    }
}
