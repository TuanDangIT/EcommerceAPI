using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class ProductParameter : BaseEntity<int>
    {
        public Product Product { get; private set; } = new();
        public Guid ProductId { get; private set; }
        public Parameter Parameter { get; set; } = new();
        public Guid ParameterId { get; private set; }
        public string Value { get; set; } = string.Empty;
    }
}
