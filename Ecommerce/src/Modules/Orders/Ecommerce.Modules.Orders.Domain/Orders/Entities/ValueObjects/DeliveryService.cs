using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Entities.ValueObjects
{
    public class DeliveryService
    {
        public string Courier { get; private set; } = string.Empty;
        public string Service { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public DeliveryService(string courier, string service, decimal price)
        {
            Courier = courier;
            Service = service;
            Price = price;
        }
    }
}
