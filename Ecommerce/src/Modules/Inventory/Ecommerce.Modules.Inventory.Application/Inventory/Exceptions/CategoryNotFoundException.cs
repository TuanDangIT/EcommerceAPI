using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Exceptions
{
    internal class CategoryNotFoundException : EcommerceException
    {
        public Guid Id { get; }
        public CategoryNotFoundException(Guid id) : base($"Category: {id} was not found.")
        {
            Id = id;
        }
    }
}
