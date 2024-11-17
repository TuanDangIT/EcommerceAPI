using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects
{
    public class Weight
    {
        //public int Id { get; set; }
        public string Amount { get; private set; } = string.Empty;
        public string Unit { get; private set; } = "kg";
        public Weight(string amount)
        {
            Amount = amount;
        }
        public Weight()
        {

        }
    }
}
