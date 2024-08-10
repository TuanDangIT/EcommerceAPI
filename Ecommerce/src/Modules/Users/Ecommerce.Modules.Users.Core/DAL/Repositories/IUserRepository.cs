using Ecommerce.Modules.Users.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetAsync(Guid id);
        Task<User?> GetAsync(string email);
        Task AddAsync(User user);
        Task<RefreshToken> AddRefreshTokenAsync(User user, string generatedRefreshToken);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
