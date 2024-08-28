using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Domain.Inventory.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        public string ImageUrlPath { get; set; } = string.Empty;
        public int Order { get; set; }
        public Product Product { get; set; } = new();
        public Guid ProductId { get; set; }

    }
}
