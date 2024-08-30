using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    internal class Shipment
    {
        public string City { get; } = string.Empty;
        public string StreetName { get; } = string.Empty;
        public int StreetNumber { get; }
        public string ReceiverFullName { get; } = string.Empty;
        public Shipment(string city, string streetName, int streetNumber, string receiverFullName)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ShipmentNullException("City");
            }

            if (string.IsNullOrWhiteSpace(streetName))
            {
                throw new ShipmentNullException("Street name");
            }

            if (string.IsNullOrWhiteSpace(receiverFullName))
            {
                throw new ShipmentNullException("Receiver full name");
            }

            City = city;
            StreetName = streetName;
            StreetNumber = streetNumber;
            ReceiverFullName = receiverFullName;
        }
        public Shipment()
        {
            
        }
    }
}
