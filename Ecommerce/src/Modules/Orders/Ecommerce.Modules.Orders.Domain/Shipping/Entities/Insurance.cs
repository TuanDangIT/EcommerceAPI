using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Shipping.Entities
{
    public class Insurance
    {
        //public int Id { get; set; }
        public string Amount { get; private set; }
        public string Currency { get; private set; } = "PLN";
        public Insurance(string amount)
            => Amount = amount;
    }
}
