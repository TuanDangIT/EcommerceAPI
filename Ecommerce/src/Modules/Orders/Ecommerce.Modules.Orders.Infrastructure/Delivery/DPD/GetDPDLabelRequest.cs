using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery.DPD
{
    public class GetDPDLabelRequest
    {
        public LabelSearchParams LabelSearchParams { get; set; }
        public string OutputDocFormat { get; set; } = "PDF";
        public string Format { get; set; } = "A4";
        public string OutputType { get; set; } = "BIC3";
        public string Variant { get; set; } = "STANDARD";
        public GetDPDLabelRequest(int sessionId)
        {
            LabelSearchParams = new LabelSearchParams(sessionId);
        }
    }
}
