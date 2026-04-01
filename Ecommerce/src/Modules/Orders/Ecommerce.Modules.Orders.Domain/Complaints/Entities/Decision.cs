using Ecommerce.Modules.Orders.Domain.Complaints.Exceptions;
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
        public Decision(Complaint complaint, string decisionText, string? additionalInformation, decimal refundAmount)
        {
            if(refundAmount < 0)
            {
                throw new RefundAmountBelowZeroException();
            }

            if(refundAmount > complaint.Order.Products.Sum(p => p.Price))
            {
                throw new InvalidAmountToReturnException();
            }
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
