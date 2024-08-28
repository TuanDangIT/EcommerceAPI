using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class ProductParameter
    {
        public int Id { get; set; }
        public Product Product { get; set; } = new();
        public Guid ProductId { get; set; }
        public Parameter Parameter { get; set; } = new();
        public Guid ParameterId { get; set; }
        public string Value { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
