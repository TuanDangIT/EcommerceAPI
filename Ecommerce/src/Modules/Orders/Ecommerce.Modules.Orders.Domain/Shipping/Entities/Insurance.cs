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
        public string Amount { get; set; }
        public string Currency { get; set; } = "PLN";
        public Insurance(string amount)
            => Amount = amount;
    }
}
