using Ecommerce.Modules.Users.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Entities
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public UserType Type { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Username {  get; set; } = string.Empty;
        public Role Role { get; set; } = new();
        public int RoleId { get; set; }
        public RefreshToken? RefreshToken { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public int FailedAttempts { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public User(Guid id, string fullName, string email, string password, string username, Role role, DateTime createdAt)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            Password = password;
            Username = username;
            Role = role;
            CreatedAt = createdAt;
        }
        public User()
        {
            
        }
    }
}
