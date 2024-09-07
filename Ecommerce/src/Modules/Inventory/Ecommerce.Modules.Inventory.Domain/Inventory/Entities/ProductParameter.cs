using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class ProductParameter
    {
        public int Id { get; private set; }
        public Product Product { get; private set; } = new();
        public Guid ProductId { get; private set; }
        public Parameter Parameter { get; private set; } = new();
        public Guid ParameterId { get; private set; }
        public string Value { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
    }
}
