using Ecommerce.Modules.Orders.Domain.Orders.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities
{
    public class ShipmentDetails
    {
        public string City { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;
        public string StreetName { get; private set; } = string.Empty;
        public string StreetNumber { get; private set; } = string.Empty;
        public string ApartmentNumber { get; private set; } = string.Empty;
        public ShipmentDetails(string city, string postalCode, string streetName, string streetNumber, string apartmentNumber)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ShipmentNullException(nameof(City));
            }

            if (string.IsNullOrWhiteSpace(postalCode))
            {
                throw new ShipmentNullException("Postal code");
            }

            if (string.IsNullOrWhiteSpace(streetName))
            {
                throw new ShipmentNullException("Street name");
            }

            if (string.IsNullOrWhiteSpace(streetNumber))
            {
                throw new ShipmentNullException("Street number");
            }

            City = city;
            PostalCode = postalCode;
            StreetName = streetName;
            StreetNumber = streetNumber;
            ApartmentNumber = apartmentNumber;
        }
        public ShipmentDetails()
        {

        }
    }
}
