using Ecommerce.Modules.Users.Core.Entities.Enums;
using Ecommerce.Shared.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Entities
{
    public abstract class User : BaseEntity<Guid>, IAuditable
    {
        public UserType Type { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Username {  get; set; } = string.Empty;
        public Role Role { get; set; } = new();
        public int RoleId { get; set; }
        public RefreshToken? RefreshToken { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public int FailedAttempts { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        private const int _maxFailedAccessAttempts = 5;
        private const int _lockoutTimeSpan = 5;
        public User(Guid id, string firstName, string lastName, string email, string password, string username, Role role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Username = username;
            Role = role;
        }
        public User()
        {
            
        }
        public bool IsLockedOut()
        {
            if (TimeProvider.System.GetUtcNow() > LockoutEnd)
            {
                return false;
            }
            return true;
        }
        public void ResetFailedAttempts()
            => FailedAttempts = 0;
        public void ResetLockoutEnd()
            => LockoutEnd = null;
        public void SignInFailed()
        {
            FailedAttempts++;
            if (FailedAttempts > _maxFailedAccessAttempts)
            {
                LockOut();
            }
        }
        private void LockOut()
            => LockoutEnd = TimeProvider.System.GetUtcNow().UtcDateTime + TimeSpan.FromMinutes(_lockoutTimeSpan);
    }
}
