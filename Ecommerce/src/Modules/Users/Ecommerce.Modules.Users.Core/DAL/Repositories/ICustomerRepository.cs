using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Shared.Infrastructure.Pagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DAL.Repositories
{
    public interface ICustomerRepository
    {
        Task<PagedResult<CustomerBrowseDto>> GetAllAsync(SieveModel model);
        Task<Customer?> GetAsync(Guid customerId, bool asNoTracking);
    }
}
