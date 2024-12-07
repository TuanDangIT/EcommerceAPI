using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Domain.Orders.Repositories
{
    public interface IShipmentRepository
    {
        Task<Shipment?> GetAsync(int shipmentId, CancellationToken cancellationToken = default);
    }
}
