using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Exceptions
{
    internal class ManufacturerNotUpdated : EcommerceException
    {
        public Guid Id { get; }
        public ManufacturerNotUpdated(Guid id) : base($"Manufacturer: {id} was not updated.")
        {
            Id = id;
        }
    }
}
