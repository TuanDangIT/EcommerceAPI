using Ecommerce.Modules.Carts.Core.Entities.Enums;
using Ecommerce.Modules.Carts.Core.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Entities
{
    public class DeliveryService
    {
        public int Id { get; set; }
        public Courier Courier { get; set; } 
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public bool IsActive { get; set; } = true;
        public DeliveryService(int id, Courier courier, string name, decimal price)
        {
            Id = id;
            Courier = courier;
            Name = name;
            Price = price;
        }
        private DeliveryService()
        {
            
        }
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
