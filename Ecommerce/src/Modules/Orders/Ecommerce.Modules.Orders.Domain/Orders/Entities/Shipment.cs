using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class Shipment
    {
        public string City { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;
        public string StreetName { get; private set; } = string.Empty;
        public string StreetNumber { get; private set; } = string.Empty;
        public string AparmentNumber { get; private set; } = string.Empty;
        public Shipment(string city, string postalCode, string streetName, string streetNumber, string apartmentNumber)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ShipmentNullException("City");
            }

            if (string.IsNullOrWhiteSpace(streetName))
            {
                throw new ShipmentNullException("Street name");
            }
            City = city;
            PostalCode = postalCode;
            StreetName = streetName;
            StreetNumber = streetNumber;
            AparmentNumber = apartmentNumber;
        }
        public Shipment()
        {

        }
    }
}
