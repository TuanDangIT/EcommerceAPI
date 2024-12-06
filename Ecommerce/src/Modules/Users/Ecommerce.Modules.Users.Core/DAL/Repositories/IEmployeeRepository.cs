using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DAL.Repositories
{
    public interface IEmployeeRepository
    {
        Task<PagedResult<EmployeeBrowseDto>> GetAllAsync(SieveModel model, CancellationToken cancellationToken = default);
        Task<Employee?> GetAsync(Guid employeeId, bool asNoTracking, CancellationToken cancellationToken = default);
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);
    }
}
