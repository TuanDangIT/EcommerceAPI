using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Shipping.Entities
{
    public class Dimensions
    {
        //public int Id { get; set; }
        public string Length { get; set; } = string.Empty;
        public string Width { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Unit { get; set; } = "mm";
        public Dimensions(string length, string width, string height)
        {
            Length = length;
            Width = width;
            Height = height;
        }
        public Dimensions()
        {
            
        }
    }
}
