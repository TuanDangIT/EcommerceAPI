using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Shipping.Entities
{
    public class Weight
    {
        //public int Id { get; set; }
        public string Amount { get; set; } = string.Empty;
        public string Unit { get; set; } = "kg";
        public Weight(string amount)
        {
            Amount = amount;
        }
        public Weight()
        {
            
        }
    }
}
