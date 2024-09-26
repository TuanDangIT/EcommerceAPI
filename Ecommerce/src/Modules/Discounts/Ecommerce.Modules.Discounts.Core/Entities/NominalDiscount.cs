using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public class NominalDiscount : Discount
    {
        public decimal NominalValue { get; set; }
        public NominalDiscount(string code, decimal nominalValue, DateTime? endingDate, DateTime createdAt) : base(code, endingDate, createdAt)
        {
            NominalValue = nominalValue; 
        }
        public NominalDiscount(string code, decimal nominalValue, DateTime createdAt) : base(code, createdAt)
        {
            NominalValue = nominalValue;
        }
    }
}
