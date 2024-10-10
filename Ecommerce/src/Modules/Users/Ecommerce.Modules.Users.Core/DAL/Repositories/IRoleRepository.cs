using Ecommerce.Modules.Users.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DAL.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetAsync(string roleName);
        Task<Role?> GetAsync(int roleId);
    }
}
