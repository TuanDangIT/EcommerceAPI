using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects
{
    public class Address
    {
        //public int Id { get; set; }
        public string Street { get; private set; } = string.Empty;
        public string BuildingNumber { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string PostCode { get; private set; } = string.Empty;
        public string CountryCode { get; private set; } = "PL";
        public string Country { get; private set; } = string.Empty;
        public Address(string country, string street, string buildingNumber, string city, string postCode)
        {
            Country = country;
            Street = street;
            BuildingNumber = buildingNumber;
            City = city;
            PostCode = postCode;
        }
        public Address()
        {

        }
    }
}
