using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Entities
{
    public class Employee : User
    {
        public string JobPosition { get; set; } = string.Empty;
        public Employee(Guid id, string fullName, string email, string password, string username, Role role, string jobPosition, DateTime createdAt) 
            : base(id, fullName, email, password, username, role, createdAt)
        {
            JobPosition = jobPosition;
        }
        public Employee()
        {
            
        }
    }
}
