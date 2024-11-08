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
        public Employee(Guid id, string firstName, string lastName, string email, string password, string username, Role role, string jobPosition) 
            : base(id, firstName, lastName, email, password, username, role)
        {
            JobPosition = jobPosition;
        }
        public Employee()
        {
            
        }
    }
}
