using Ecommerce.Modules.Users.Core.DAL;
using Ecommerce.Modules.Users.Core.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Services
{
    internal class RoleService : IRoleService
    {
        private readonly UsersDbContext _dbContext;

        public RoleService(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<RoleBrowseDto>> BrowseAsync(CancellationToken cancellationToken = default)
            => await _dbContext.Roles
            .AsNoTracking()
            .Select(r => new RoleBrowseDto()
            {
                Name = r.Name,
            })
            .ToListAsync(cancellationToken);
    }
}
