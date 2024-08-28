using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Exceptions
{
    internal class ManufacturerNotUpdatedException : EcommerceException
    {
        public Guid Id { get; }
        public ManufacturerNotUpdatedException(Guid id) : base($"Manufacturer: {id} was not updated.")
        {
            Id = id;
        }
    }
}
