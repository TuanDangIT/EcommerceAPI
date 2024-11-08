using Ecommerce.Modules.Users.Core.DAL.Repositories;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Events;
using Ecommerce.Modules.Users.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Auth;
using Ecommerce.Shared.Abstractions.Messaging;
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
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMessageBroker _messageBroker;
        private readonly TimeProvider _timeProvider;
        private readonly IAuthManager _authManager;
        //private const int _maxFailedAccessAttempts = 5;
        //private const int _lockoutTimeSpan = 5;
        private const string _customerRoleName = "Customer";

        public IdentityService(IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher<User> passwordHasher, 
            IMessageBroker messageBroker, TimeProvider timeProvider, IAuthManager authManager)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _messageBroker = messageBroker;
            _timeProvider = timeProvider;
            _authManager = authManager;
        }
        public async Task<JsonWebToken> SignInAsync(SignInDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            var now = _timeProvider.GetUtcNow().UtcDateTime;
            if (user is null)
            {
                throw new InvalidCredentialsException();
            }
            if(user.LockoutEnd is not null && user.IsLockedOut())
            {
                throw new UserLockedOutException((TimeSpan)(user.LockoutEnd - now));
            }
            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if(passwordResult is PasswordVerificationResult.Failed)
            {
                user.SignInFailed();
                await _userRepository.UpdateAsync();
                throw new InvalidCredentialsException();
            }
            if(user.IsActive is false)
            {
                throw new UserNotActiveException(user.Id);
            }
            if(user.LockoutEnd is not null)
            {
                user.ResetLockoutEnd();
            }
            if(user.FailedAttempts > 0)
            {
                user.ResetFailedAttempts();
            }
            var jwt = _authManager.GenerateAccessToken(user.Id.ToString(), user.Username, user.Role.Name);
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
            var role = await _roleRepository.GetAsync(_customerRoleName);
            var newGuid = Guid.NewGuid();
            var customer = new Customer(newGuid, dto.FirstName, dto.LastName, email, password, dto.Username, role!);
            await _userRepository.AddAsync(customer);
            await _messageBroker.PublishAsync(new CustomerActivated(newGuid, email, customer.FirstName, customer.LastName));
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
        //public async Task DeleteAsync(Guid id)
        //{
        //    await _userRepository.DeleteAsync(id);
        //}


        //public async Task UpdateAsync(UserDto userDto)
        //{
        //    await _userRepository.UpdateAsync(new User()
        //    {
        //        Id = userDto.Id,
        //        Email = userDto.Email,
        //        Role = userDto.Role,
        //        UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime
        //    });
        //}
        public async Task<UserDto?> GetAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user is null ? null : new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role.Name,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
            };
        }

        //public async Task<UserDto?> GetAsync(string email)
        //{
        //    var user = await _userRepository.GetByEmailAsync(email);
        //    return user is null ? null : new UserDto()
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        Role = user.Role.Name,
        //        CreatedAt = user.CreatedAt,
        //        UpdatedAt = user.UpdatedAt,
        //    };
        //}

    }
}
