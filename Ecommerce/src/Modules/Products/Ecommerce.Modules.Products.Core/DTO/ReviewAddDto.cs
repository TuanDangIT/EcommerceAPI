using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Products.Core.DTO
{
    public class ReviewAddDto
    {
        public string Username { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Grade { get; set; }
        public Guid ProductId { get; set; }
    }
}
