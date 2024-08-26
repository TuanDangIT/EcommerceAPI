using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Grade { get; set; }
        public Product Product { get; set; } = new();
        public Guid ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}
    }
}
