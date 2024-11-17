using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects
{
    public class Dimensions
    {
        //public int Id { get; set; }
        public string Length { get; private set; } = string.Empty;
        public string Width { get; private set; } = string.Empty;
        public string Height { get; private set; } = string.Empty;
        public string Unit { get; private set; } = "mm";
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
