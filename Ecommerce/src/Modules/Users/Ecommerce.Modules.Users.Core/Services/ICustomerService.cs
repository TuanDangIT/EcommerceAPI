using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Services
{
    public interface ICustomerService
    {
        Task DeleteAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<PagedResult<CustomerBrowseDto>> BrowseAsync(SieveModel model, CancellationToken cancellationToken = default);
        Task<CustomerDetailsDto> GetAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task UpdateAsync(CustomerUpdateDto dto, CancellationToken cancellationToken = default);
        Task SetActiveAsync(Guid customerId, bool isActive, CancellationToken cancellationToken = default);
    }
}
