using Ecommerce.Modules.Users.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default, params Expression<Func<User, object>>[] includes);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default, params Expression<Func<User, object>>[] includes);
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task<RefreshToken> AddRefreshTokenAsync(User user, string generatedRefreshToken, CancellationToken cancellationToken = default);
        Task UpdateAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
