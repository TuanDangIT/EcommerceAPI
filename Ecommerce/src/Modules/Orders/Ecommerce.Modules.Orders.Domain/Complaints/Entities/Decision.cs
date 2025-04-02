using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Complaints.Entities
{
    public class Decision
    {
        public string DecisionText { get; private set; } = string.Empty;
        public string? AdditionalInformation { get; private set; } = string.Empty;
        public decimal? RefundAmount { get; private set; }
        public Decision(string decisionText, string? additionalInformation, decimal refundAmount)
        {
            DecisionText = decisionText;
            AdditionalInformation = additionalInformation;
            RefundAmount = refundAmount;

        }
        public Decision(string decisionText, string? additionalInformation)
        {
            DecisionText = decisionText;
            AdditionalInformation = additionalInformation;

        }
        public Decision()
        {
            
        }
        public void EditDecision(string decisionText, string? additionalInformation)
        {
            DecisionText = decisionText;
            AdditionalInformation = additionalInformation;
        }
    }
}
