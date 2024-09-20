using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Entities
{
    public class Decision
    {
        public string DecisionText { get; set; } = string.Empty;
        public string? AdditionalInformation { get; set; } = string.Empty;
        public Decision(string decisionText, string additionalInformation)
        {
            DecisionText = decisionText;
            AdditionalInformation = additionalInformation;
        }
        public Decision()
        {
            
        }
    }
}
