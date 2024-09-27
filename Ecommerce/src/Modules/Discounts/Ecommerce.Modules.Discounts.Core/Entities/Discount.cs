using Ecommerce.Modules.Discounts.Core.Entities.Enums;
using Ecommerce.Modules.Discounts.Core.Entities.Exceptions;
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
        protected Discount(string code, DateTime endingDate, DateTime createdAt)
        {
            Code = code;
            CreatedAt = createdAt;
            if(EndingDate < TimeProvider.System.GetUtcNow().UtcDateTime)
            {
                throw new DiscountInvalidEndingDateException(endingDate);
            }
            EndingDate = endingDate;
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
