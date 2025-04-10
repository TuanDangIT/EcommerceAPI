﻿using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Entities.Enums;
using Ecommerce.Modules.Users.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
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
        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<RefreshToken> AddRefreshTokenAsync(User user, string generatedRefreshToken, CancellationToken cancellationToken = default)
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
            await _dbContext.SaveChangesAsync(cancellationToken);
            return refreshToken;
        }

        public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default, params Expression<Func<User, object>>[] includes)
        {
            var query = _dbContext.Users
                .AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var user = query
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            return user;
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default, params Expression<Func<User, object>>[] includes)
        {
            var query = _dbContext.Users
                .AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var user = query.
                FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
            return user;
        }

        public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default) 
            => _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
