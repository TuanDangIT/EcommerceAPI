using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities.ValueObjects
{
    public class Shipment
    {
        public string Country { get; set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;
        public string StreetName { get; private set; } = string.Empty;
        public string StreetNumber { get; private set; } = string.Empty;
        public string? AparmentNumber { get; private set; }
        public string Service { get; set; } = "Kurier InPost";
        public decimal Price { get; set; } = 3;
        public Shipment(string country, string city, string postalCode, string streetName, string streetNumber, string? apartmentNumber)
        {
            Country = country;
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
