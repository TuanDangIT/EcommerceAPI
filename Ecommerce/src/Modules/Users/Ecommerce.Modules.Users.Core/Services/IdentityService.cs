using Ecommerce.Modules.Users.Core.DAL.Repositories;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TimeProvider _timeProvider;
        private readonly IAuthManager _authManager;

        public IdentityService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, TimeProvider timeProvider, IAuthManager authManager)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _timeProvider = timeProvider;
            _authManager = authManager;
        }
        public async Task<JsonWebToken> SignInAsync(SignInDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if(user is null)
            {
                throw new InvalidCredentialsException();
            }
            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if(passwordResult is PasswordVerificationResult.Failed)
            {
                throw new InvalidCredentialsException();
            }
            if(user.IsActive is false)
            {
                throw new UserNotActiveException(user.Id);
            }
            var jwt = _authManager.GenerateAccessToken(user.Id.ToString(), user.Username);
            var generatedRefreshToken = _authManager.GenerateRefreshToken();
            var refreshToken = await _userRepository.AddRefreshTokenAsync(user, generatedRefreshToken);
            jwt.RefreshTokenExpiryTime = refreshToken.RefreshTokenExpiryTime;
            jwt.RefreshToken = refreshToken.Token;
            return jwt;

        }

        public async Task SignUpAsync(SignUpDto dto)
        {
            var email = dto.Email.ToLowerInvariant();
            if(await _userRepository.GetByEmailAsync(email) is not null)
            {
                throw new EmailInUseException();
            }
            if(await _userRepository.GetByUsernameAsync(dto.Username) is not null)
            {
                throw new UsernameInUseException();
            }
            var password = _passwordHasher.HashPassword(default!, dto.Password);
            var user = new User()
            {
                Id = dto.Id,
                Email = email,
                Password = password,
                Username = dto.Username,
                Role = dto.Role?.ToLowerInvariant() ?? "Customer",
                CreatedAt = _timeProvider.GetUtcNow().UtcDateTime,
                IsActive = true,
            };
            await _userRepository.AddAsync(user);
        }
        public async Task<JsonWebToken> RefreshTokenAsync(TokenDto dto)
        {
            string accessToken = dto.AccessToken;
            string refreshToken = dto.RefreshToken;
            var principal = _authManager.GetPrincipalFromExpiredToken(accessToken);
            var user = await _userRepository.GetByIdAsync(Guid.Parse(principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value));
            if (user is null || user.RefreshToken is null || user.RefreshToken.Token != refreshToken || user.RefreshToken.RefreshTokenExpiryTime <= _timeProvider.GetUtcNow().UtcDateTime)
            {
                throw new InvalidRefreshToken();
            }
            var jwt = _authManager.GenerateAccessToken(user.Id.ToString(), user.Username);
            var generatedRefreshToken = _authManager.GenerateRefreshToken();
            var newRefreshToken = await _userRepository.AddRefreshTokenAsync(user, generatedRefreshToken);
            jwt.RefreshTokenExpiryTime = newRefreshToken.RefreshTokenExpiryTime;
            jwt.RefreshToken = newRefreshToken.Token;
            return jwt;
        }
        public async Task DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }


        public async Task UpdateAsync(UserDto userDto)
        {
            await _userRepository.UpdateAsync(new User()
            {
                Id = userDto.Id,
                Email = userDto.Email,
                Role = userDto.Role,
                LastUpdatedAt = _timeProvider.GetUtcNow().UtcDateTime
            });
        }
        public async Task<UserDto?> GetAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user is null ? null : new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                LastUpdatedAt = user.LastUpdatedAt,
            };
        }

        public async Task<UserDto?> GetAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user is null ? null : new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                LastUpdatedAt = user.LastUpdatedAt,
            };
        }
    }
}
