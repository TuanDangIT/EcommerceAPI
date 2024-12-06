using Ecommerce.Modules.Carts.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Core.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDto>> BrowseAsync(CancellationToken cancellationToken = default);
        Task SetActiveAsync(Guid paymentId, bool isActive, CancellationToken cancellationToken = default);
        Task<IEnumerable<PaymentDto>> BrowseAvailableAsync(CancellationToken cancellationToken = default);
    }
}
