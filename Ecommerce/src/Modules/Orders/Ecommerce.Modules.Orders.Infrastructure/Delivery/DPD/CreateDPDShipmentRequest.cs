using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery.DPD
{
    internal class CreateDPDShipmentRequest
    {
        public string GenerationPolicy { get; set; } = string.Empty;
        public DPDPackage[] Packages { get; set; } = [];
        public string LangCode { get; set; } = string.Empty;
        public string OutputDocFormat { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public string OutputType { get; set; } = string.Empty;
        public string Variant { get; set; } = string.Empty;
    }
}
