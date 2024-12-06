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
    internal interface IEmployeeService
    {
        Task<Guid> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid employeeId, CancellationToken cancellationToken = default);
        Task<PagedResult<EmployeeBrowseDto>> BrowseAsync(SieveModel model, CancellationToken cancellationToken = default);
        Task<EmployeeDetailsDto> GetAsync(Guid employeeId, CancellationToken cancellationToken = default);
        Task UpdateAsync(EmployeeUpdateDto dto, CancellationToken cancellationToken = default);
        Task SetActiveAsync(Guid employeeId, bool isActive, CancellationToken cancellationToken = default);
    }
}
