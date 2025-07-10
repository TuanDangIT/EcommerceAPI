using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery.DPD
{
    public class LabelSearchParams
    {
        public string Policy { get; set; } = "STOP_ON_FIRST_ERROR";
        public Session Session { get; set; }
        public string DocumentId { get; set; } = "string";
        public LabelSearchParams(int sessionId)
        {
            Session = new Session(sessionId);
        }
    }
}
