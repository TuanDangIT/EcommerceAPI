using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects
{
    public class Discount
    {
        public DiscountType Type { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public decimal? Value { get; private set; }
        public string? SKU { get; private set; }
        public Discount(DiscountType type, string code, decimal? value, string? sku)
        {
            Type = type;
            Code = code;
            Value = value;
            SKU = sku;
        }
        private Discount()
        {

        }
    }
}
