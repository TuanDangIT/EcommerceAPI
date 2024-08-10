using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Exceptions
{
    internal class ManufacturerNotDeletedException : EcommerceException
    {
        public Guid Id { get; }
        public ManufacturerNotDeletedException(Guid id) : base($"Manufacturer: {id} was not deleted.")
        {
            Id = id;    
        }
    }
}
