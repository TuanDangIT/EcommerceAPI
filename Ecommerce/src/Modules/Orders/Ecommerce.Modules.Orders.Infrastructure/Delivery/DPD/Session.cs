using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery.DPD
{
    public class Session
    {
        public int SessionId { get; set; } 
        public string Type { get; set; } = "DOMESTIC";
        public Session(int sessionId)
        {
            SessionId = sessionId;
        }
    }
}
