using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Shipping.Entities
{
    public class Parcel
    {
        //public int Id { get; set; }
        public Dimensions Dimensions { get; private set; } = new();
        public Weight Weight { get; private set; } = new();
        public Parcel(Dimensions dimensions, Weight weight)
        {
            Dimensions = dimensions;
            Weight = weight;
        }
        public Parcel()
        {
            
        }
    }
}
