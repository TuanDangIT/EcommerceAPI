using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public class PercentageDiscount : Discount
    {
        public decimal Percent { get; set; }
        public PercentageDiscount(string code, decimal percent, DateTime? endingDate, DateTime createdAt) : base(code, endingDate, createdAt)
        {
            Percent = percent;
        }
        public PercentageDiscount(string code, decimal percent, DateTime createdAt) : base(code, createdAt)
        {
            Percent = percent;
        }
    }
}
