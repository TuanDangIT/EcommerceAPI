using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery.DPD
{
    public class DPDPackage
    {
        public DPDReceiver Receiver { get; set; } = default!;
        public DPDSender Sender { get; set; } = default!;
        [JsonPropertyName("payerFID")]
        public int PayerFID { get; set; }
        public DPDParcel[] Parcels { get; set; } = default!;
    }
}
