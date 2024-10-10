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
    public interface ICustomerService
    {
        Task DeleteAsync(Guid customerId);
        Task<PagedResult<CustomerBrowseDto>> BrowseAsync(SieveModel model);
        Task<CustomerDetailsDto> GetAsync(Guid customerId);
        Task UpdateAsync(CustomerUpdateDto dto);
        Task SetActiveAsync(Guid customerId, bool isActive);
    }
}
