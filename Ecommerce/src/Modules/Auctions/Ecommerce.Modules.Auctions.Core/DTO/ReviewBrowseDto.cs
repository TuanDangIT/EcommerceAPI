using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Auctions.Core.DTO
{
    public class ReviewBrowseDto
    {
        public string Username { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Grade { get; set; }
    }
}
