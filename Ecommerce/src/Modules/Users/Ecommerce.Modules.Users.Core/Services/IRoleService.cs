using Ecommerce.Modules.Users.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleBrowseDto>> BrowseAsync(CancellationToken cancellationToken = default);
    }
}
