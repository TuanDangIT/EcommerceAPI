using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery.DPD
{
    public class DPDParcel
    {
        public decimal SizeZ { get; set; }
        public decimal SizeX { get; set; }
        public decimal SizeY { get; set; }
        public decimal Weight { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
