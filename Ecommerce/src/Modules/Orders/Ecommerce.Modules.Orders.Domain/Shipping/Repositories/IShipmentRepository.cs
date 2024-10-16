using Ecommerce.Modules.Orders.Domain.Shipping.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Shipping.Repositories
{
    public interface IShipmentRepository
    {
        Task<Shipment?> GetAsync(int shipmentId);
    }
}
