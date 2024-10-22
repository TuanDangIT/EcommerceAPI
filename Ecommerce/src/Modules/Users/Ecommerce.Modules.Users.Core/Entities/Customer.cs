using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Entities
{
    public class Customer : User
    {
        public Customer(Guid id, string firstName, string lastName, string email, string password, string username, Role role, DateTime createdAt) : base(id, firstName, lastName, email, password, username, role, createdAt)
        {
        }
        public Customer() 
        {
            
        }
    }
}
