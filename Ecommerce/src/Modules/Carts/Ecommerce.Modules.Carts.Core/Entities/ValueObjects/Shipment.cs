using Ecommerce.Modules.Carts.Core.Entities.Exceptions;
using Ecommerce.Modules.Carts.Core.Exceptions;
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
        public int DeliveryServiceId { get; private set; }
        public DeliveryService DeliveryService { get; private set; } = default!;
        public Shipment(string country, string city, string postalCode, string streetName, string streetNumber, string? apartmentNumber, DeliveryService deliveryService)
        {
            Country = country;
            City = city;
            PostalCode = postalCode;
            StreetName = streetName;
            StreetNumber = streetNumber;
            AparmentNumber = apartmentNumber;
            ChooseDeliveryService(deliveryService);
        }
        private Shipment()
        {

        }
        public void ChooseDeliveryService(DeliveryService deliveryService)
        {
            if (deliveryService.IsActive == false)
            {
                throw new DeliveryServiceNotActiveException(deliveryService.Id);
            }
            DeliveryService = deliveryService;
        }
    }
}
