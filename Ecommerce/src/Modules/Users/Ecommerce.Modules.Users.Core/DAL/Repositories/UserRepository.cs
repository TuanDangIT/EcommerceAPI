using Ecommerce.Modules.Users.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.DAL.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _dbContext;
        private readonly TimeProvider _timeProvider;

        public UserRepository(UsersDbContext dbContext, TimeProvider timeProvider)
        {
            _dbContext = dbContext;
            _timeProvider = timeProvider;
        }
        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> AddRefreshTokenAsync(User user, string generatedRefreshToken)
        {
            var userId = user.Id;
            var expiryTime = _timeProvider.GetUtcNow().AddDays(2).UtcDateTime;
            var refreshToken = new RefreshToken()
            {
                UserId = userId,
                Token = generatedRefreshToken,
                RefreshTokenExpiryTime = expiryTime
            };
            user.RefreshToken = refreshToken;
            await _dbContext.SaveChangesAsync();
            return refreshToken;
        }
        public async Task DeleteAsync(Guid id)
        {
            var user = new User()
            {
                Id = id
            };
            var entry = _dbContext.Attach(user);
            entry.State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public Task<User?> GetByIdAsync(Guid id) => _dbContext.Users.SingleOrDefaultAsync(x => x.Id == id);

        public Task<User?> GetByEmailAsync(string email) => _dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);

        public Task<User?> GetByUsernameAsync(string username) => _dbContext.Users.SingleOrDefaultAsync(x => x.Username == username);

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
        public Task<List<User>> GetAllAsync() => _dbContext.Users.ToListAsync();
    }
}
