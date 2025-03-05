using Ecommerce.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DAL.Repositories
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly UsersDbContext _dbContext;

        public RoleRepository(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Role?> GetAsync(string roleName, CancellationToken cancellationToken = default)
            => await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken);
    }
}
