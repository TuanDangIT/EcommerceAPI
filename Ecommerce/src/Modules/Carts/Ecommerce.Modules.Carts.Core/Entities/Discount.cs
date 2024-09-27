using Ecommerce.Modules.Carts.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class Discount
    {
        public int Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public DiscountType Type { get; private set; }
        public decimal Value { get; private set; }
        public DateTime? EndingDate { get; private set; }
        private readonly List<CheckoutCart> _checkoutCarts = [];
        public IEnumerable<CheckoutCart> CheckoutCarts => _checkoutCarts;
        public Discount(string code, DiscountType type, decimal value, DateTime? endingDate)
        {
            Code = code;
            Type = type;
            Value = value;
            EndingDate = endingDate;

        }
        public Discount()
        {
            
        }
    }
}
