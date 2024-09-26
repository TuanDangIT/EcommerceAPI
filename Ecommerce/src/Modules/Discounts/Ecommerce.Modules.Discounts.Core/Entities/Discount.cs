using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.Entities
{
    public abstract class Discount
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DiscountType Type { get; set; }
        public DateTime? EndingDate { get; set; }
        public DateTime CreatedAt { get; set; }
        protected Discount(string code, DateTime? endingDate, DateTime createdAt)
        {
            Code = code;
            EndingDate = endingDate;
            CreatedAt = createdAt;
        }
        protected Discount(string code, DateTime createdAt)
        {
            Code = code;
            CreatedAt = createdAt;
        }
        protected Discount()
        {
            
        }
    }
}
