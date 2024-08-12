using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Exceptions
{
    internal class CategoryNotDeletedException : EcommerceException
    {
        public Guid Id { get; }
        public CategoryNotDeletedException(Guid id) : base($"Category: {id} was not deleted.")
        {
            Id = id;
        }
    }
}
