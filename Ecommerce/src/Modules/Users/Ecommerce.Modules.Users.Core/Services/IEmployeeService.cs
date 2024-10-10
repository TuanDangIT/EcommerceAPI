using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Shared.Infrastructure.Pagination;
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
        Task<Guid> CreateAsync(EmployeeCreateDto dto);
        Task DeleteAsync(Guid employeeId);
        Task<PagedResult<EmployeeBrowseDto>> BrowseAsync(SieveModel model);
        Task<EmployeeDetailsDto> GetAsync(Guid employeeId);
        Task UpdateAsync(EmployeeUpdateDto dto);
        Task SetActiveAsync(Guid employeeId, bool isActive);
    }
}
