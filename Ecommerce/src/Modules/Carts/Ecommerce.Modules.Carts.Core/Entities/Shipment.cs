﻿using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
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
            AparmentNumber = apartmentNumber;
        }
        public Shipment()
        {
            
        }
    }
}
