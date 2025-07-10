using Ecommerce.Modules.Carts.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services
{
    public interface IDeliveryServiceService
    {
        Task<IEnumerable<DeliveryServiceDto>> GetAllAsync(bool? isActive, CancellationToken cancellationToken = default);
        Task SetActiveAsync(int deliveryServiceId, bool isActive, CancellationToken cancellationToken = default);
    }
}
